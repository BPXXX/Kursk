using UnityEngine;
using UnityEditor;
using System.Collections;

using System;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using System.IO;
using GameFramework.Hotfix.Editor;
using GameFramework;

public class GenSprotoEntity : ScriptableObject
{
    private static Dictionary<string, string> m_mapType = new Dictionary<string, string>();
    private static Dictionary<string, bool> m_filterClassMap = new Dictionary<string, bool>();
    
    /// <summary>
    /// 更好的做法其实应该用文件模板来做,TODO
    /// </summary>
    /// <param name="data.{0}"></param>
    /// <returns></returns>
    private static string m_dicHandler = "{1}if (et.{0} == null) {{1}\t et.{0} = new {2}();{1}}{1}foreach(var item in data.{0}){ {1}\tif(et.{0}.ContainsKey(item.Key)){{1}\t\tet.{0}[item.Key] = item.Value;{1}\t}else{{1}\t\tet.{0}.Add(item.Key, item.Value);{1}\t}{1}}{1}ET.ATTR.Add(\"{0}\"); \n\t\t\t}\n";

    static List<Type> GetClasses(string nameSpace)
    {
        m_mapType["System.String"] = "string";
        m_mapType["System.Boolean"] = "bool";
        m_mapType["System.Int64"] = "Int64";
        m_mapType["System.Byte[]"] = "byte[]";

        m_filterClassMap["package"] = true;
        m_filterClassMap["Header"] = true;
        m_filterClassMap["ErrorMessage"] = true;
        m_filterClassMap["MessageContent"] = true;
        m_filterClassMap["GateMessage"] = true;



        List<Type> classlist = new List<Type>();
        
        var bytes= File.ReadAllBytes(HotfixCodeGen.HotDllOutputPath);

        //bytes = IRes.FileDecode(bytes);

		Assembly asm = Assembly.Load(bytes);

        //foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        //{
            foreach (Type type in asm.GetTypes())
            {
                if (type.Namespace == nameSpace)
                {
                    PropertyInfo[] props = type.GetProperties();
                    foreach (var item in props)
                    {
                        
                        if ( m_mapType.ContainsKey(item.PropertyType.ToString()) == false)
                        {
                            if (item.PropertyType.ToString().IndexOf("[") > 0)
                            {
                                Regex reg = new Regex(@"(?<=\[)[^]]*(?=\])");
                                Match m = reg.Match(item.PropertyType.ToString());
                                if (m.Success)
                                {
                                    string newClassName = m.ToString();
                                    if(newClassName.IndexOf(",")>0){
                                        newClassName = newClassName.Substring(newClassName.IndexOf(",")+1);
                                    }

                                    Type t = asm.GetType(newClassName);
                                    if (t !=null &&  m_mapType.ContainsKey(newClassName) == false)
                                    {
                                        classlist.Add(t);
                                        UnityEngine.Debug.Log(t.ToString());
                                    }else{
                                        UnityEngine.Debug.LogFormat("get type error {0} {1}",newClassName,item.PropertyType.ToString());
                                    }
                                }
                            }
                            else
                            {
                                UnityEngine.Debug.Log(item.PropertyType.ToString());
                                classlist.Add(item.PropertyType);
                            }
                        }
                    }
                }
            }
        //}
        return classlist;
    }


    //[MenuItem("Tools/Sproto/Gen Sproto Entity")]
    public static void createEntity()
    {
        //Assembly asm;
        List<Type> classs = GetClasses("SprotoType");


        StringBuilder sb = new StringBuilder();


        sb.Append("using UnityEngine;");
        sb.AppendLine();

        sb.Append("using System.Collections.Generic;");
        sb.AppendLine();

        sb.Append("using System;");
        sb.AppendLine();

        sb.Append("using SprotoType;");
        sb.AppendLine();

        sb.Append("namespace Game");
        sb.AppendLine();

        sb.Append("{");
        sb.AppendLine();

        Dictionary<string, bool> hasGenClass = new Dictionary<string, bool>();
        UnityEngine.Debug.Log("++++++++++++++++++++++");
        foreach (var cls in classs)
        {
            if (cls == null)
            {
                continue;
            }
            UnityEngine.Debug.Log(cls.ToString());
            string className = cls.ToString();
            if (cls.ToString().LastIndexOf("+")>0)
            {
                className = className.Substring(cls.ToString().LastIndexOf("+") + 1); 
            }

            if(className.LastIndexOf(".")>0){
                className = className.Substring(className.LastIndexOf(".") + 1); 
            }

            

            if (m_filterClassMap.ContainsKey(className) == false && hasGenClass.ContainsKey(className) == false){

                hasGenClass[className] = true;

                //构造类
                string line = string.Format("\tpublic class {0}Entity\n\t{{\n", className);
                sb.Append(line);

                sb.Append(string.Format("\t\tpublic const string {0}Change = \"{1}Change\";\n",className,className));

                StringBuilder fsb = new StringBuilder();

                foreach (System.Reflection.PropertyInfo p in cls.GetProperties())
                {
                    if (p.Name.IndexOf("Has") == -1)
                    {
                        string str = p.PropertyType.ToString();
                        str = Regex.Replace(str, "`\\d", "").Replace("[", "<").Replace("]", ">").Replace("+", ".");
                        str = Regex.Replace(str, "Byte<>", "Byte[]");
                        //TODO  多个内嵌需要优化
                        sb.Append(string.Format("\t\tpublic {0} {1};\n", str, p.Name));

                        fsb.Append(string.Format("\t\t\tif(data.Has{0}){{\n", p.Name[0].ToString().ToUpper() + p.Name.Substring(1)));
                        if (str.Contains("Dictionary"))
                        {   
                            fsb.Append(m_dicHandler.Replace("{0}",p.Name).Replace("{1}","\n\r\t\t\t\t").Replace("{2}",str));
                        }else {
                            fsb.Append(string.Format("\t\t\t\tet.{0} = data.{1};\n\t\t\t\tif(ret)ET.ATTR.Add(\"{2}\");\n\t\t\t}}\n"
                                                , p.Name, p.Name, p.Name ));
                        }
                    }
                }
                //start function
                sb.Append(string.Format("\n\t\tpublic static HashSet<string> updateEntity({0}Entity et ,{1} data,bool ret = false){{\n\t\t\tif(ret)ET.ATTR.Clear();\n", className,cls.ToString().Replace("+",".")));

                //function content
                sb.Append(fsb.ToString());

                //end function
                sb.Append("\t\t\treturn ET.ATTR;\n\t\t}\n");

                sb.Append("\t}\n");
            }
        }

        //end namespace
        sb.Append("}");
        sb.AppendLine();

        //UnityEngine.Debug.Log(sb.ToString());


        string filepath = Application.dataPath + "/Scripts/Hotfix/Protocol/SprotoEntity.cs";
        FileInfo t = new FileInfo(filepath);
        if (!File.Exists(filepath))
        {
            File.Delete(filepath);
        }
        StreamWriter sw = t.CreateText();


        sw.WriteLine(sb.ToString());
        sw.Close();
        sw.Dispose();

        //TODO zj
        //DllBuild.BuildHitfixDll(true,false);

    }
}