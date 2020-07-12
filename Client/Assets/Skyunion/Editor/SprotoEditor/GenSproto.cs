#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class SprotoGen : AssetPostprocessor
{

	public static readonly string prefProtocEnable = "SprotoUnity_Enable";
	public static readonly string prefProtocExecutable = "SprotoUnity_ProtocExecutable";
	
	public static readonly string prefProtoDir = "SprotoUnity_ProtocDir";
	public static readonly string prefLogError = "SprotoUnity_LogError";
	public static readonly string prefLogStandard = "SprotoUnity_LogStandard";

	public static bool enabled
	{
		get
		{
			return EditorPrefs.GetBool(prefProtocEnable, true);
		}
		set
		{
			EditorPrefs.SetBool(prefProtocEnable, value);
		}
	}
	public static bool logError
	{
		get
		{
			return EditorPrefs.GetBool(prefLogError, true);
		}
		set
		{
			EditorPrefs.SetBool(prefLogError, value);
		}
	}

	public static bool logStandard
	{
		get
		{
			return EditorPrefs.GetBool(prefLogStandard, false);
		}
		set
		{
			EditorPrefs.SetBool(prefLogStandard, value);
		}
	}

	public static string excPath
	{
		get
		{
			string luaExe = Application.dataPath.Replace("Assets", "") + "/tool/lua/lua";
			if (File.Exists(luaExe))
			{
#if UNITY_EDITOR_OSX
				return luaExe;
#elif UNITY_EDITOR_WIN
				return luaExe+".exe";
#endif
			}
			
			return EditorPrefs.GetString(prefProtocExecutable, "");
		}
		set
		{
			EditorPrefs.SetString(prefProtocExecutable, value);
		}
	}
	
	
	public static string SprotoPath
	{
		get
		{
			
			return EditorPrefs.GetString(prefProtoDir, "");
		}
		set
		{
			EditorPrefs.SetString(prefProtoDir, value);
		}
	}

	[PreferenceItem("Sproto")]
	static void PreferencesItem()
	{
		EditorGUI.BeginChangeCheck();


		enabled = EditorGUILayout.Toggle(new GUIContent("Enable Sproto Compilation", ""), enabled);

		EditorGUI.BeginDisabledGroup(!enabled);

		EditorGUILayout.HelpBox(@"On Windows put the path to lua.exe (e.g. C:\My Dir\lua.exe), on macOS and Linux you can use ""which lua"" to find its location. (e.g. /usr/local/bin/lua)", MessageType.Info);

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("lua执行文件路径", GUILayout.Width(100));
		excPath = EditorGUILayout.TextField(excPath, GUILayout.ExpandWidth(true));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("同步协议目录", GUILayout.Width(100));
		SprotoPath = EditorGUILayout.TextField(SprotoPath, GUILayout.ExpandWidth(true));
		EditorGUILayout.EndHorizontal();

		logError = EditorGUILayout.Toggle(new GUIContent("Log Error Output", "Log compilation errors from protoc command."), logError);

		logStandard = EditorGUILayout.Toggle(new GUIContent("Log Standard Output", "Log compilation completion messages."), logStandard);

		EditorGUILayout.Space();

		if (GUILayout.Button(new GUIContent("Force Compilation")))
		{
			CompileAllInProject();
		}

		EditorGUI.EndDisabledGroup();

		if (EditorGUI.EndChangeCheck())
		{
		}
	}


    /// <summary>
    /// 重新生成Sproto协议
    /// </summary>
    [MenuItem("Tools/Sproto/生成协议CS文件")]
    private static void CompileAllInProject()
	{
		if (logStandard)
		{
			UnityEngine.Debug.Log("Sproto Unity : Compiling all .sproto files in the project...");
		}
		string[] protoFiles = Directory.GetFiles(Application.dataPath, "*.sproto", SearchOption.AllDirectories);
		foreach (string s in protoFiles)
		{
			if (logStandard)
			{
				UnityEngine.Debug.Log("Sproto Unity : Compiling " + s);
			}
			CompileProtobufSystemPath(s);
		}
	}
    
    
    /// <summary>
    /// 同步Sproto协议
    /// </summary>
    [MenuItem("Tools/Sproto/同步协议 & 生成协议 & 生成实体")]
    private static void SyscSproto()
    {
	    if (!Directory.Exists(SprotoPath))
	    {
		    Debug.LogError("未检测到lua环境请指定Preferences => Sproto => 同步协议目录");
		    return;
	    }
	    string[] protoFiles = Directory.GetFiles(SprotoPath, "*.sproto", SearchOption.AllDirectories);
	    foreach (string s in protoFiles)
	    {
		    var targetFile = Application.dataPath + "/Protocol/" + Path.GetFileName(s);
		    
			UnityEngine.Debug.LogFormat("拷贝协议文件{0}=>{1}",s,targetFile);
		   
		    File.Copy(s,targetFile,true);
		    
		    CompileProtobufSystemPath(targetFile);
	    }
	    
	    GenSprotoEntity.createEntity();
    }

    
    

	private static void CompileProtobufAssetPath(string assetPath)
	{
		string systemPath = Directory.GetParent(Application.dataPath) + Path.DirectorySeparatorChar.ToString() + assetPath;
		CompileProtobufSystemPath(systemPath);
	}

	private static void CompileProtobufSystemPath(string systemPath)
	{

		if (Path.GetExtension(systemPath) == ".sproto")
		{
            //TODO  window path
            string refreshPath = "/Scripts/Hotfix/Protocol/Sproto.cs";
            string outputPath = Application.dataPath+ refreshPath;

			const string options = " sprotodump.lua -cs {0} -o {1}";

			if (string.IsNullOrEmpty(excPath))
			{
				Debug.LogError("未检测到lua环境请指定Preferences => Sproto => Path to protoc lua执行文件路径");
				return;
			}

			ProcessStartInfo startInfo = new ProcessStartInfo() { FileName = excPath, Arguments = string.Format(options, systemPath, outputPath) };
            startInfo.WorkingDirectory = Directory.GetParent(Application.dataPath)+Path.DirectorySeparatorChar.ToString() + "tool";

			Process proc = new Process() { StartInfo = startInfo };
            proc.StartInfo.UseShellExecute = false;
			proc.StartInfo.RedirectStandardOutput = true;
			proc.StartInfo.RedirectStandardError = true;

			proc.Start();

			string output = proc.StandardOutput.ReadToEnd();
			string error = proc.StandardError.ReadToEnd();
			proc.WaitForExit();

            //刷新资源管理器

			if (logStandard)
			{
				if (output != "")
				{
					UnityEngine.Debug.Log("Sproto Unity Output : " + output);
				}
                UnityEngine.Debug.Log("End Sproto Unity : Compiled " + Path.GetFileName(systemPath));
			}

			if (logError && error != "")
			{
				UnityEngine.Debug.LogError("End Sproto Unity : " + error);
            }else{
				//TODO zj
				UnityEngine.Debug.Log("生成协议成功"+new DateTime().ToString());
                //DllBuild.BuildHitfixDll(true,false);
            }

		}
	}


}

#endif
