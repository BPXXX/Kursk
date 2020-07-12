using UnityEngine;
using UnityEditor;
using System.IO;
public class ExportSprite
{

    [MenuItem("Assets/导出选中图片为单独png")]
    static void ExportSelSprite()
    {
        try
        {
            AssetDatabase.StartAssetEditing();
            string outPath = "Assets/SpritesOutput";
            foreach (Object obj in Selection.objects)
            {
                string loadPath = AssetDatabase.GetAssetPath(obj);
                string ext = System.IO.Path.GetExtension(loadPath);
                string[] allFiles = Directory.GetFiles(loadPath);
                // 加载此文件下的所有资源
                foreach (var item in allFiles)
                {
                    Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(item);
                    if (sprites.Length > 0)
                    {
                        string tmpOutPath = outPath + "/" + obj.name;
                        if (!System.IO.Directory.Exists(tmpOutPath))
                        {
                            System.IO.Directory.CreateDirectory(tmpOutPath);
                        }
                        foreach (var element in sprites)
                        {
                            if (element is Sprite)
                            {
                                // 创建单独的纹理
                                Sprite sprite = element as Sprite;
                                SerializedObject so = new SerializedObject(sprite.texture);
                                var readable = so.FindProperty("m_IsReadable");
                                if (readable != null)
                                {
                                    readable.boolValue = true;
                                    so.ApplyModifiedProperties();
                                }
                                Texture2D tex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.ARGB32, false);
                                tex.SetPixels(sprite.texture.GetPixels((int)sprite.rect.xMin, (int)sprite.rect.yMin,
                                    (int)sprite.rect.width, (int)sprite.rect.height));
                                tex.Apply();

                                // 写入成PNG文件
                                System.IO.File.WriteAllBytes(tmpOutPath + "/" + sprite.name + ".png", tex.EncodeToPNG());
                            }
                        }
                        Debug.Log(string.Format("Export {0} to {1}", loadPath, outPath));
                    }
                }


            }
        }
        finally
        {
            AssetDatabase.Refresh();
            AssetDatabase.StopAssetEditing();
        }
        Debug.Log("Export All Sprites Finished");
    }
}
