using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using Skyunion;
using GameFramework.Hotfix.Editor;

public static class ILRuntimeCLRBinding
{
    //[MenuItem("Tools/ILRuntime/Generate CLR Binding Code")]
    static void GenerateCLRBinding()
    {
        List<Type> types = new List<Type>();
        types.Add(typeof(int));
        types.Add(typeof(float));
        types.Add(typeof(long));
        types.Add(typeof(object));
        types.Add(typeof(string));
        types.Add(typeof(Array));
        types.Add(typeof(Vector2));
        types.Add(typeof(Vector3));
        types.Add(typeof(Quaternion));
        types.Add(typeof(GameObject));
        types.Add(typeof(UnityEngine.Object));
        types.Add(typeof(Transform));
        types.Add(typeof(RectTransform));
        types.Add(typeof(Time));
        types.Add(typeof(Debug));
        types.Add(typeof(Activator));
        //所有DLL内的类型的真实C#类型都是ILTypeInstance
        types.Add(typeof(List<ILRuntime.Runtime.Intepreter.ILTypeInstance>));

        ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(types, "Assets/Scripts/ILRTBind/Generated/AutoType");
		AssetDatabase.Refresh();
    }

    [MenuItem("Skyunion/ILRuntime/Generate CLR Binding Code by Analysis")]
    static void GenerateCLRBindingByAnalysis()
    {
        GenerateCLRBinding();

        //用新的分析热更dll调用引用来生成绑定代码
        ILRuntime.Runtime.Enviorment.AppDomain domain = new ILRuntime.Runtime.Enviorment.AppDomain();
        var hotfixDllSourcePath = Path.Combine(System.Environment.CurrentDirectory, "Library/ScriptAssemblies/Hotfix.dll");
        using (FileStream fs = new FileStream(hotfixDllSourcePath, FileMode.Open, FileAccess.Read))
        {
	        domain.LoadAssembly(fs);
            //Crossbind Adapter is needed to generate the correct binding code
            ILRTBind.ILRTBind.Init(domain);
            ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(domain, "Assets/Scripts/ILRTBind/Generated/AutoType");
	        AssetDatabase.Refresh();
        }

    }
    [MenuItem("Skyunion/ILRuntime/Generate Adaptor&&Delegate")]
    public static void genILAPT()
    {
        HotfixCodeGen.Init();

        AssetDatabase.Refresh();
    }
}
