using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CodeGenerationTools.Generator;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Skyunion;
using UnityEditor;
using UnityEngine;

public static class ILRuntimeGenCode
{
    private static string _outputPath;

    private static readonly Dictionary<string, TypeDefinition> _adaptorDic = new Dictionary<string, TypeDefinition>();
    private static readonly Dictionary<string, TypeDefinition> _delegateCovDic = new Dictionary<string, TypeDefinition>();
    private static readonly Dictionary<string, TypeReference> _delegateRegDic = new Dictionary<string, TypeReference>();

    private static AdaptorGenerator _adGenerator;
    private static HelperGenerator _helpGenerator;


    private static string _adaptorAttrName = "ILRuntime.Other.NeedAdaptorAttribute";
    private static string _delegateAttrName = "ILRuntime.Other.DelegateExportAttribute";


    //[MenuItem("Skyunion/ILRuntime/Generate Adaptor&Delegate")]
    static void GenerateCLRBindingByAnalysis()
    {
        _outputPath = Path.Combine(Application.dataPath, "Skyunion/Runtime/HotFixService/ILRuntime/Generated2/");
        _adaptorDic.Clear();
        _delegateCovDic.Clear();
        _delegateRegDic.Clear();

        LoadTemplates();

        //LoadMainProjectAssembly(Application.dataPath.Replace("Assets", "") + "/Library/ScriptAssemblies/Assembly-CSharp.dll");
        LoadMainProjectAssembly(Application.dataPath.Replace("Assets", "") + "/Library/ScriptAssemblies/Skyunion.dll");
        LoadILScriptAssembly(Application.dataPath.Replace("Assets", "") + "/Library/ScriptAssemblies/Hotfix.dll");

        Generate();

        AssetDatabase.Refresh();
    }

    private static void LoadMainProjectAssembly(string dllPath)
    {
        try
        {
            var module = ModuleDefinition.ReadModule(dllPath);
            if (module == null) return;
            
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
            Debug.Log("----------main assembly loaded---------" + dllPath);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    private static void LoadILScriptAssembly(string targetPath)
    {
        try
        {
            using (var fs = File.Open(targetPath, FileMode.Open))
            {
                _delegateRegDic.Clear();
                var module = ModuleDefinition.ReadModule(fs);
                foreach (var typeDefinition in module.Types)
                {
                    foreach (var methodDefinition in typeDefinition.Methods)
                    {
                        if (methodDefinition?.Body?.Instructions == null)
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
                            Debug.Log("[delegate register]" + typeName);
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

    private static void Generate()
    {
        if (_adaptorDic.Count <= 0 && _delegateCovDic.Count <= 0 && _delegateRegDic.Count <= 0)
        {
            Debug.Log("[Warnning] There is nothing to Generate");
            return;
        }

        Debug.Log("===============================Clear Old Files================================");
        var files = Directory.GetFiles(_outputPath);
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

    #region CodeGenerate Methods
    private static void LoadTemplates()
    {
        var tmpdPath = Application.dataPath + "/Skyunion/Editor/ILRuntimeEditor/Template/";

        _adGenerator = new AdaptorGenerator();
        _adGenerator.LoadTemplateFromFile(tmpdPath + "adaptor.tmpd");

        _helpGenerator = new HelperGenerator();
        _helpGenerator.LoadTemplateFromFile(tmpdPath + "helper.tmpd");
    }


    private static void CreateILRuntimeHelper()
    {
        Debug.Log($"==================Begin create helper:=====================");

        _helpGenerator.LoadData(new Tuple<Dictionary<string, TypeDefinition>, Dictionary<string, TypeDefinition>, Dictionary<string, TypeReference>>(_adaptorDic, _delegateCovDic, _delegateRegDic));
        var helperStr = _helpGenerator.Generate();

        using (var fs2 = File.Create(_outputPath + "helper.cs"))
        {
            var sw = new StreamWriter(fs2);
            sw.Write(helperStr);
            sw.Flush();
        }

        Debug.Log($"==============End create helper:===================");
    }

    private static void CreateAdaptor(TypeDefinition type)
    {
        if (type.IsInterface)
            return;


        Debug.Log($"================begin create adaptor:{type.Name}=======================");

        var adaptorName = type.Name + "Adaptor";

        using (var fs = File.Create(_outputPath + adaptorName + ".cs"))
        {

            _adGenerator.LoadData(type);
            var classbody = _adGenerator.Generate();

            var sw = new StreamWriter(fs);
            sw.Write(classbody);
            sw.Flush();
        }

        Debug.Log($"================end create adaptor:{type.Name}=======================");
    }

    private static void LoadDelegateRegister(string key, TypeReference type)
    {
        if (!_delegateRegDic.ContainsKey(key))
            _delegateRegDic.Add(key, type);
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
    #endregion CodeGenerate Methods
}
