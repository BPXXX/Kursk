using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace Client
{
    public static class ColorExtend
    {
        public static Color AverageColor(this Color[] colors)
        {
            Color color = new Color(0, 0, 0, 0);
            for (int i = 0; i < colors.Length; i++)
            {
                color += colors[i];
            }
            color /= colors.Length;
            return color;
        }
    }


    //图像工具
    //1.压缩
    //2.翻转
    //3.保存和读取
    public class ImageUtil
    {
        public enum FlipType
        {
            UpDownFlip,
            LeftRightFlip,
            AllFlip
        }

        public static bool CompressFromPath(string originPath, string targetPath, float proportion)
        {
            try
            {
                string extension = Path.GetExtension(originPath).ToLowerInvariant();
                TextureFormat format = (extension == ".jpg" || extension == ".jpeg") ? TextureFormat.RGB24 : TextureFormat.ARGB32;
                Texture2D originTexture = new Texture2D(2, 2, format, true);
                if (!originTexture.LoadImage(File.ReadAllBytes(originPath), false))
                {
                    UnityEngine.Object.DestroyImmediate(originTexture);
                    return false;
                }
                float xWidth = originTexture.width * proportion;
                float yHeight = originTexture.height * proportion;
                Texture2D result =  Compress(originTexture, proportion);
                if (result == null)
                {
                    return false;
                }
                else if (format == TextureFormat.RGB24)
                {
                    File.WriteAllBytes(targetPath, result.EncodeToJPG());
                }
                else
                {
                    File.WriteAllBytes(targetPath, result.EncodeToPNG());
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                return false;
            }
        }

        public static Texture2D Compress(Texture2D originTexture,float proportion)
        {
            if (originTexture = null)
            {
                return null;
            }
            try
            {
                float xWidth = originTexture.width * proportion;
                float yHeight = originTexture.height * proportion;
                float multiple = 1f / proportion;
                Texture2D result = new Texture2D((int)xWidth, (int)yHeight);

                for (int y = 0; y < result.height; y++)
                {
                    for (int x = 0; x < result.width; x++)
                    {
                        int xPos = (int)(x * multiple);
                        int yPos = (int)(y * multiple);

                        //if (y>0 &&y< result.height-1&&x>0&&x<result.width-1)
                        //{
                        Color[] colors = originTexture.GetPixels(xPos, yPos, 3, 3);
                        result.SetPixel(x, y, colors.AverageColor());
                        //}
                    }
                }
                result.Apply();
                return result;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                return null;
            }
        }

        public static Texture2D Flip(Texture2D texture, FlipType flipType)
        {
            if(texture==null)
            {
                return texture;
            }
            Texture2D result = new Texture2D(texture.width, texture.height);
            bool upDownFlip = flipType == FlipType.UpDownFlip || flipType == FlipType.AllFlip;
            bool leftRightFlip = flipType == FlipType.LeftRightFlip||flipType==FlipType.AllFlip;
            for (int y =  0; y < texture.height;y++)
            {
                int yPos = upDownFlip ? y : texture.height - y - 1;
                for (int x = 0;x<texture.width;x++)
                {
                    int xPos = leftRightFlip ? x : texture.width - x -1;
                    result.SetPixel(x,y,texture.GetPixel(xPos,yPos));
                }
            }
            result.Apply();
            return result;
        }

        public static Texture2D ReadFromFile(string path,bool unreadable = false)
        {
            try
            {
                Texture2D result = new Texture2D(2, 2);
                result.LoadImage(File.ReadAllBytes(path),unreadable);
                result.Apply();
                return result;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                return null;
            }
        }

        public static bool SaveImageFile(string path,Texture2D target)
        {
            if(string.IsNullOrEmpty(path)||target==null)
            {
                return false;
            }
            try
            {
                string dir = Path.GetDirectoryName(path);
                string extension = Path.GetExtension(path).ToLowerInvariant();
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                if((extension == ".jpg" || extension == ".jpeg"))
                {
                    File.WriteAllBytes(path, target.EncodeToJPG());
                }
                else
                {
                    File.WriteAllBytes(path, target.EncodeToPNG());
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                return false;
            }
        }
    }
}