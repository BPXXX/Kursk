using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Reflection;

using CodeGenerationTools.Generator;
using CodeGenerationTools.Generator.Base;
using ILRuntime.Other;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System;

namespace GameFramework.Hotfix.Editor
{
    public class HotfixCodeGen
    {
        public static string MainDLLPath = Application.dataPath + "/../Library/ScriptAssemblies/Skyunion.dll"; 
        public static string HotDllOutputPath = Application.dataPath + "/../Library/ScriptAssemblies/Hotfix.dll";
        public static string GenILTypeCodePath = Application.dataPath + "/Scripts/ILRTBind/Generated/AutoType/";
        public static string GenILAdaptorCodePath = Application.dataPath + "/Scripts/ILRTBind/Generated/AutoAdaptor/";

        private static readonly Dictionary<string, TypeDefinition> _adaptorDic = new Dictionary<string, TypeDefinition>();
        private static readonly Dictionary<string, TypeDefinition> _delegateCovDic = new Dictionary<string, TypeDefinition>();
        private static readonly Dictionary<string, TypeReference> _delegateRegDic = new Dictionary<string, TypeReference>();

        private static AdaptorGenerator _adGenerator;
        private static HelperGenerator _helpGenerator;

        private static string _adaptorAttrName = "ILRuntime.Other.NeedAdaptorAttribute";
        private static string _delegateAttrName = "ILRuntime.Other.DelegateExportAttribute";

        private static bool m_isInited = false;

        public static void Init(){

                LoadTemplates();
                LoadMainProjectAssembly();
                LoadILScriptAssembly();

                GenCode();

        }


        public static void GenCode()
        {
            if (_adaptorDic.Count <= 0 && _delegateCovDic.Count <= 0 && _delegateRegDic.Count <= 0)
            {
                Debug.Log("[Warnning] There is nothing to Generate");
                return;
            }

            Debug.Log("===============================Clear Old Files================================");
            if (!Directory.Exists(GenILAdaptorCodePath))
            {
                Directory.CreateDirectory(GenILAdaptorCodePath);
            }
            var files = Directory.GetFiles(GenILAdaptorCodePath);
            foreach (var file in files)
            {
                File.Delete(file);
            }

            Debug.Log("[=============================Generate Begin==============================]");

            foreach (var type in _adaptorDic.Values)
            {
                CreateAdaptor(type);
            }

            CreateILRuntimeHelper();

            Debug.Log("[=============================Generate End=================================]");
        }


        private static void LoadTemplates()
        {
            var tmpdPath = Application.dataPath + "/Skyunion/Editor/ILRuntimeEditor/Template/";

            _adGenerator = new AdaptorGenerator();
            _adGenerator.LoadTemplateFromFile(tmpdPath + "adaptor.tmpd");
            _helpGenerator = new HelperGenerator();
            _helpGenerator.LoadTemplateFromFile(tmpdPath + "helper.tmpd");

        }


        private static void CreateAdaptor(TypeDefinition type)
        {
            if (type.IsInterface)
                return;


            Debug.Log(string.Format("================begin create adaptor:{0}=======================", type.Name));

            var adaptorName = type.Name + "Adaptor";

            using (var fs = File.Create(GenILAdaptorCodePath + adaptorName + ".cs"))
            {

                _adGenerator.LoadData(type);
                var classbody = _adGenerator.Generate();

                var sw = new StreamWriter(fs);
                sw.Write(classbody);
                sw.Flush();
            }

            Debug.Log(string.Format("================end create adaptor:{0}=======================", type.Name));

        }

        private static void CreateILRuntimeHelper()
        {
            Debug.Log("==================Begin create helper:=====================");

            _helpGenerator.LoadData(new Tuple<Dictionary<string, TypeDefinition>, Dictionary<string, TypeDefinition>, Dictionary<string, TypeReference>>(_adaptorDic, _delegateCovDic, _delegateRegDic));
            var helperStr = _helpGenerator.Generate();

            using (var fs2 = File.Create(GenILAdaptorCodePath + "helper.cs"))
            {
                var sw = new StreamWriter(fs2);
                sw.Write(helperStr);
                sw.Flush();
            }

            Debug.Log("==============End create helper:===================");
        }

        public static void LoadMainProjectAssembly()
        {
            var targetPath = MainDLLPath;


            try
            {
                var module = ModuleDefinition.ReadModule(targetPath);
                if (module == null) return;

                _adaptorDic.Clear();
                _delegateCovDic.Clear();

                var typeList = module.GetTypes();
                foreach (var t in typeList)
                {
                    foreach (var customAttribute in t.CustomAttributes)
                    {
                        if (customAttribute.AttributeType.FullName == _adaptorAttrName)
                        {
                            Debug.Log("[Need Adaptor]" + t.FullName);
                            LoadAdaptor(t);
                            continue;
                        }

                        if (customAttribute.AttributeType.FullName == _delegateAttrName)
                        {
                            //unity dll egg hurt name has '/'
                            var typeName = t.FullName.Replace("/", ".");
                            Debug.Log("[Delegate Export]" + typeName);
                            LoadDelegateConvertor(t);
                            continue;
                        }
                    }
                }


                //var assembly = Assembly.LoadFrom(targetPath);
                //if (assembly == null) return;

                ////如果自定义属性用自定义
                //_adaptorAttr = assembly.GetType("ILRuntime.Other.NeedAdaptorAttribute");
                //if (_adaptorAttr == null) _adaptorAttr = typeof(NeedAdaptorAttribute);

                //_delegateAttr = assembly.GetType("ILRuntime.Other.DelegateExportAttribute");
                //if (_delegateAttr == null) _delegateAttr = typeof(DelegateExportAttribute);

                ////types
                //Type[] types;
                //try
                //{
                //    types = assembly.GetTypes();
                //}
                //catch (ReflectionTypeLoadException ex)
                //{
                //    types = ex.Types;
                //}
                //catch (IOException ioex)
                //{
                //    Print(ioex.Message);
                //    types = new Type[0];
                //}

                //foreach (var type in types)
                //{
                //    if (type == null) continue;
                //    //load adaptor
                //    if (type == _adaptorAttr)
                //        continue;

                //    //var attr = type.GetCustomAttribute(typeof(NeedAdaptorAttribute), false);
                //    //if (attr.Length > 0)
                //    var attr = type.GetCustomAttribute(_adaptorAttr, false);
                //    if (attr != null)
                //    {
                //        Print("[adaptor]" + type.FullName);
                //        LoadAdaptor(type);
                //        continue;
                //    }
                //    if (type == _delegateAttr)
                //        continue;
                //    //load delegate
                //    //var attr1 = type.GetCustomAttributes(typeof(DelegateExportAttribute), false);
                //    //if (attr1.Length > 0)
                //    var attr1 = type.GetCustomAttribute(_delegateAttr, false);
                //    if (attr1 != null)
                //    {
                //        Print("[delegate convertor]" + type.FullName);
                //        LoadDelegateConvertor(type);
                //    }
                //}
                Debug.Log("----------main assembly loaded");
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }


        public static void LoadILScriptAssembly()
        {

            var targetPath = HotDllOutputPath;


            try
            {
                using (var fs = File.Open(targetPath, FileMode.Open))
                {
                    var module = ModuleDefinition.ReadModule(fs);
                    foreach (var typeDefinition in module.Types)
                    {
                        foreach (var methodDefinition in typeDefinition.Methods)
                        {
                            if (methodDefinition == null || methodDefinition.Body == null || methodDefinition.Body.Instructions == null)
                                continue;
                            foreach (var instruction in methodDefinition.Body.Instructions)
                            {
                                if (instruction.OpCode != OpCodes.Newobj || instruction.Previous == null ||
                                    instruction.Previous.OpCode != OpCodes.Ldftn) continue;

                                var type = instruction.Operand as MethodReference;
                                if (type == null ||
                                    (!type.DeclaringType.Name.Contains("Action") &&
                                     !type.DeclaringType.Name.Contains("Func"))) continue;

                                var typeName = type.DeclaringType.FullName;//.Replace("/", ".");

                                LoadDelegateRegister(typeName, type.DeclaringType);
                            }
                        }
                    }
                }

                Debug.Log("----------ilscript assembly loaded");
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }

        }

        private static void LoadDelegateRegister(string key, TypeReference type)
        {
            if (!_delegateRegDic.ContainsKey(key))
            {
                _delegateRegDic.Add(key, type);
                Debug.Log("[delegate register]" + key);
            }
            else
                _delegateRegDic[key] = type;
        }

        private static void LoadDelegateConvertor(TypeDefinition type)
        {
            var key = type.FullName.Replace("/", ".");
            if (!_delegateCovDic.ContainsKey(key))
                _delegateCovDic.Add(key, type);
            else
                _delegateCovDic[type.FullName] = type;
        }

        private static void LoadAdaptor(TypeDefinition type)
        {
            //var key = type.FullName.Replace("/", ".");
            if (!_adaptorDic.ContainsKey(type.FullName))
                _adaptorDic.Add(type.FullName, type);
            else
                _adaptorDic[type.FullName] = type;
        }
    }


     
}