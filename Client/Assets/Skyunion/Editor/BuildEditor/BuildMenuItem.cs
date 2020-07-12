//
// AssetsMenuItem.cs
//
// Author:
//       fjy <jiyuan.feng@live.com>
//
// Copyright (c) 2019 fjy
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.Assertions;

namespace Skyunion
{
    public static class BuildMenuItem
    {
        [MenuItem("Skyunion/Build/BuildContent")]
        private static void BuildContent()
        {
            //LogAssert.ignoreFailingMessages = true;
            //AddressableAssetSettings oldSettings = AddressableAssetSettingsDefaultObject.Settings;
            //AddressableAssetSettingsDefaultObject.Settings = Settings;

            //bool callbackCalled = false;
            //BuildScript.buildCompleted += (result) =>
            //{
            //    callbackCalled = true;
            //};
            //AddressableAssetSettings.BuildPlayerContent();
            //Assert.IsTrue(callbackCalled);

            //if (oldSettings != null)
            //    AddressableAssetSettingsDefaultObject.Settings = oldSettings;
            //AddressableAssetSettings.BuildPlayerContent();
            //UnityEngine.TestTools.LogAssert.ignoreFailingMessages = false;

            //BuildScript.BuildStandalonePlayer();

            AddressableAssetSettings.BuildPlayerContent();
        }

        [MenuItem("Skyunion/Build/BuildPlayer")]
        private static void BuildStandalonePlayer()
        {
            var levels = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray();
            if (levels.Length == 0)
            {
                Debug.Log("Nothing to build.");
                return;
            }
            var targetName = GetBuildTargetName(EditorUserBuildSettings.activeBuildTarget);
            if (targetName == null)
                return;
            var outputPath = Application.dataPath + "/../../Packages/";
            Debug.Log("BuildTarget:"+EditorUserBuildSettings.activeBuildTarget);
            if(EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                outputPath += "Android/";
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows || EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64)
            {
                outputPath += "Windows/";
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneOSX)
            {
                outputPath += "OSX/";
            }
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = levels,
                locationPathName = outputPath + targetName,
                target = EditorUserBuildSettings.activeBuildTarget,
                options = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None
            };
            BuildPipeline.BuildPlayer(buildPlayerOptions);
        }

        [MenuItem("Skyunion/Build/一键打包")]
        private static void BuildPackage()
        {
            BuildContent();
            BuildStandalonePlayer();
        }

        private static string GetBuildTargetName(BuildTarget target)
        {
            var name = PlayerSettings.productName + "_" + PlayerSettings.bundleVersion;
            switch (target)
            {
                case BuildTarget.Android:
                    return name + PlayerSettings.Android.bundleVersionCode + ".apk";

                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return name + PlayerSettings.Android.bundleVersionCode + ".exe";

#if UNITY_2017_3_OR_NEWER
                case BuildTarget.StandaloneOSX:
                    return name + ".app";

#else
                    case BuildTarget.StandaloneOSXIntel:
                    case BuildTarget.StandaloneOSXIntel64:
                    case BuildTarget.StandaloneOSXUniversal:
                                        return name + ".app";

#endif

                case BuildTarget.WebGL:
                case BuildTarget.iOS:
                    return "";
                // Add more build targets for your own.
                default:
                    Debug.Log("Target not implemented.");
                    return null;
            }
        }


    }
}