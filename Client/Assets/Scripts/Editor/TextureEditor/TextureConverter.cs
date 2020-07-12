using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityEditor.UI
{
    static internal class TextureConverterMenu
    {
        private static Dictionary<string, string> m_guidMap = new Dictionary<string, string>();
        //[MenuItem("Assets/TextureTool/ConvertAssetToPng")]//右键菜单的名称
        //public static void ExportImageFile()
        //{
        //    try
        //    {
        //        m_guidMap.Clear();
        //        string assetpath = AssetDatabase.GetAssetPath(Selection.activeObject);
        //        if (Path.GetExtension(assetpath) != "")
        //        {
        //            if (Path.GetExtension(assetpath) == ".asset")
        //            {
        //                ExportPng(assetpath);
        //            }
        //        }
        //        else
        //        {
        //            ExportDir(assetpath);
        //        }
        //        AssetDatabase.Refresh();

        //        List<string> withoutExtensions = new List<string>() { ".prefab", ".unity", ".mat", ".asset" };
        //        string[] files = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories)
        //            .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();

        //        int nCount = 0;
        //        foreach (string path in files)
        //        {
        //            string content = File.ReadAllText(path);
        //            bool bHasChange = false;
        //            foreach (var pari in m_guidMap)
        //            {
        //                var key = $"guid: {pari.Key}, type: 2";
        //                if (content.IndexOf(key) != -1)
        //                {
        //                    content = content.Replace(key, $"guid: {pari.Value}, type: 3");
        //                    bHasChange = true;
        //                    Debug.Log(key);
        //                }
        //            }
        //            if (bHasChange)
        //            {
        //                File.WriteAllText(path, content);
        //            }
        //        }
        //        foreach (var pari in m_guidMap)
        //        {
        //            File.Delete(AssetDatabase.GUIDToAssetPath(pari.Key));
        //        }
        //        AssetDatabase.Refresh();
        //        Debug.Log($"Convert {m_guidMap.Count} Texture Succesed !");
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogError(e.Message + e.StackTrace);
        //    }
        //}
        [MenuItem("Assets/TextureTool/导出地形贴图")]//右键菜单的名称
        public static void ExportPNGFile()
        {
            string assetpath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (Path.GetExtension(assetpath) != "")
            {
                if (Path.GetExtension(assetpath) == ".asset")
                {
                    ExportTerrainPng(assetpath);
                }
            }
        }
        private static void ExportTerrainPng(string srcPath)
        {
            Texture2D assets = AssetDatabase.LoadAssetAtPath<Texture2D>(srcPath.Substring(srcPath.IndexOf("Assets")));
            if (assets != null)
            {
                Texture2D texture = assets;
                string assetPath = AssetDatabase.GetAssetPath(texture);

                Texture2D outputtex = new Texture2D(texture.width, texture.height, TextureFormat.RGB24, false);
                for (int y = 0; y < outputtex.height; ++y)
                {
                    for (int x = 0; x < outputtex.width; ++x)
                    {
                        Color color;
                        {
                            color = texture.GetPixel(x, y);
                        }
                        outputtex.SetPixel(x, y, color);
                    }
                }
                byte[] pngShot = outputtex.EncodeToPNG();
                string parentPath = Directory.GetParent(srcPath).FullName;
                string fileName = parentPath + "/" + Path.GetFileNameWithoutExtension(srcPath) + ".png";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                File.WriteAllBytes(fileName, pngShot);

                System.GC.Collect();
            }
        }
        private static void ExportDir(string srcPath)
        {
            foreach (string path in Directory.GetFiles(srcPath))
            {
                if (Path.GetExtension(path) == ".asset")
                {
                    ExportPng(path);
                }
            }
            foreach (string path in Directory.GetDirectories(srcPath))
            {
                ExportDir(path);
            }
        }

        private static void ExportPng(string srcPath)
        {
            Texture2D assets = AssetDatabase.LoadAssetAtPath<Texture2D>(srcPath.Substring(srcPath.IndexOf("Assets")));
            if (assets != null)
            {
                Texture2D texture = assets;
                string assetPath = AssetDatabase.GetAssetPath(texture);
                var assetGuid = AssetDatabase.AssetPathToGUID(assetPath);
                Debug.Log(assetGuid);

                SerializedObject so = new SerializedObject(assets);
                var readable = so.FindProperty("m_IsReadable");
                if (readable != null)
                {
                    readable.boolValue = true;
                    so.ApplyModifiedProperties();
                }

                Texture2D outputtex = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
                for (int y = 0; y < outputtex.height; ++y)
                {
                    for (int x = 0; x < outputtex.width; ++x)
                    {
                        Color color;
                        {
                            color = texture.GetPixel(x, y);
                        }
                        outputtex.SetPixel(x, y, color);
                    }
                }
                byte[] pngShot = outputtex.EncodeToPNG();
                string parentPath = Directory.GetParent(srcPath).FullName;
                string fileName = parentPath + "/" + texture.name + ".png";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                File.WriteAllBytes(fileName, pngShot);
                var pngGuid = AssetDatabase.AssetPathToGUID(fileName.Substring(fileName.IndexOf("Assets")));

                m_guidMap.Add(assetGuid, pngGuid);

                System.GC.Collect();
            }
        }

        private static Texture2D GetTex2D()
        {
            // Create a texture the size of the screen, RGB24 format
            int width = Screen.width;
            int height = Screen.height;
            Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
            // Read screen contents into the texture
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();
            return tex;
        }
    }
}