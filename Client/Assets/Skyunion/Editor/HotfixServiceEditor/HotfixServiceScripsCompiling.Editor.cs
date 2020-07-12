//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Skyunion
{
	[InitializeOnLoad]
	public class UnityScripsCompiling : AssetPostprocessor {
		static UnityScripsCompiling() {
			EditorApplication.update += Update;
		}
	 
		public static void OnPostprocessAllAssets(
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromAssetPaths) {
			List<string> importedKeys = new List<string>() { "Hotfix" };
			for (int i = 0; i < importedAssets.Length; i++) {
				for (int j = 0; j < importedKeys.Count; j++) {
					if (importedAssets[i].Contains(importedKeys[j])) {
						PlayerPrefs.SetInt("ImportHotfixScripts", 1);
						return;
					}
				}
			}
		}
	 
		private static void Update() {
			bool importScripts = Convert.ToBoolean(PlayerPrefs.GetInt("ImportHotfixScripts", 1));
			if (importScripts && !EditorApplication.isCompiling) {
				OnUnityScripsCompilingCompleted();
				importScripts = false;
				PlayerPrefs.SetInt("ImportHotfixScripts", 0);
				EditorApplication.update -= Update;
			}
		}
	 
		public static void OnUnityScripsCompilingCompleted()
        {
            if (EditorPrefs.GetInt(HotfixServiceEditor.HotfixDebugModeKey, (int)HotfixMode.NativeCode) == (int)HotfixMode.ILRT)
            {
                if (!Directory.Exists(Application.streamingAssetsPath))
                {
                    Directory.CreateDirectory(Application.streamingAssetsPath);
                }
                var hotfixDllSourcePath = Path.Combine(System.Environment.CurrentDirectory, "Library/ScriptAssemblies/Hotfix.dll");
                var hotfixDllTargetPath = Path.Combine(Application.streamingAssetsPath, "Hotfix.dll.bytes");
                File.Copy(hotfixDllSourcePath, hotfixDllTargetPath, true);
                var hotfixPdbSourcePath = Path.Combine(System.Environment.CurrentDirectory, "Library/ScriptAssemblies/Hotfix.pdb");
                var hotfixPdbTargetPath = Path.Combine(Application.streamingAssetsPath, "Hotfix.pdb.bytes");
                File.Copy(hotfixPdbSourcePath, hotfixPdbTargetPath, true);
                Debug.Log("Hotfix Compiling To :");
                Debug.Log(hotfixDllTargetPath);
                Debug.Log(hotfixPdbTargetPath);
                AssetDatabase.Refresh();
            }
        }
	}
}
