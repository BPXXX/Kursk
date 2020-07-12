using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skyunion;
using Spine;
using Spine.Unity;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Client
{
    public static class ClientUtils
    {
        #region 服务
        #endregion
        #region 管理器
        #endregion
        public static void ClearCore()
        {
        }

        #region 辅助函数
        //打印对象数据
        public static void Print(object obj)
        {
            if (obj != null)
            {
                Debug.Log(LitJson.JsonMapper.ToJson(obj));
            }
        }
        //资源预加载
        public static void PreLoadRes(GameObject viewObj, List<string> prefabNames, Action<Dictionary<string, GameObject>> loadFinishCallback)
        {
            GameObject tempObj = viewObj;
            Dictionary<string, GameObject> assetDic = new Dictionary<string, GameObject>();
            int total = prefabNames.Count;
            int count = 0;
            for(int i =0; i<total;i++)
            {
                CoreUtils.assetService.LoadAssetAsync<GameObject>(prefabNames[i], (asset) => {
                    if (tempObj == null)
                    {
                        assetDic.Clear();
                        return;
                    }
                    GameObject obj = asset.asset() as GameObject;
                    if (obj != null)
                    {
                        assetDic[obj.name] = obj;
                    }
                    else
                    {
                        Debug.LogError(string.Format("asset load fail :{0}", asset.ToString()));
                    }
                    count = count + 1;
                    if (count >= total)
                    {
                        loadFinishCallback(assetDic);
                    }
                });
            }
        }

        //资源预加载
        public static void PreLoadRes(GameObject viewObj, List<string> prefabNames, Action<Dictionary<string, IAsset>> loadFinishCallback)
        {
            GameObject tempObj = viewObj;
            Dictionary<string, IAsset> assetDic = new Dictionary<string, IAsset>();
            int total = prefabNames.Count;
            if (total < 1)
            {
                loadFinishCallback(assetDic);
                return;
            }
            int count = 0;
            for (int i = 0; i < total; i++)
            {
                CoreUtils.assetService.LoadAssetAsync<GameObject>(prefabNames[i], (asset) => {
                    if (tempObj == null || asset.asset() == null)
                    {
                        Debug.LogErrorFormat("load prefab fail:{0}", asset.assetName());
                        assetDic.Clear();
                        return;
                    }
                    assetDic[asset.asset().name] = asset;
                    count = count + 1;
                    if (count >= total)
                    {
                        loadFinishCallback(assetDic);
                    }
                });
            }
        }

        #endregion


        /// <summary>
        /// 替换皮肤和动作
        /// </summary>
        /// <param name="graphic"></param>
        /// <param name="skeletonDataAsset"></param>
        /// <param name="skin"></param>
        /// <returns></returns>
        private static SkeletonGraphic ChangeSkeletonGraphic (SkeletonGraphic graphic,SkeletonDataAsset skeletonDataAsset, Skin skin = null) {
            graphic.skeletonDataAsset = skeletonDataAsset;
            SkeletonData data = skeletonDataAsset.GetSkeletonData(true);

            if (data == null) {
//                for (int i = 0; i < skeletonDataAsset.atlasAssets.Length; i++) {
//                    string reloadAtlasPath = AssetDatabase.GetAssetPath(skeletonDataAsset.atlasAssets[i]);
//                    skeletonDataAsset.atlasAssets[i] = (AtlasAsset)AssetDatabase.LoadAssetAtPath(reloadAtlasPath, typeof(AtlasAsset));
//                }

                data = skeletonDataAsset.GetSkeletonData(true);
            }

            skin = skin ?? data.DefaultSkin ?? data.Skins.Items[0];
            
           
            graphic.Initialize(true);
            if (skin != null) graphic.Skeleton.SetSkin(skin);
            
            

            graphic.initialSkinName = skin.Name;

            //graphic.startingAnimation = "idle";
            graphic.Skeleton.UpdateWorldTransform();
            graphic.UpdateMesh();

            return graphic;
        }
        
        /// <summary>
        /// 加载Sprite  图片名称[Sprite名称]
        /// </summary>
        /// <param name="image"></param>
        /// <param name="assetname"></param>
        public static void LoadSprite(PolygonImage image, string assetname, bool isNativeSize = false, Action Callback = null)
        {
            CoreUtils.assetService.LoadAssetAsync<Sprite>(assetname, (asset) =>
            {
                if (asset == null)
                {
                    Debug.Log("找不到资源"+assetname);
                }
                var dat = asset.asset();
                if (dat!=null && dat is Sprite)
                {
                    image.sprite = dat as Sprite;
                    if (isNativeSize)
                    {
                        image.SetNativeSize();
                    }
                }
                else
                {
                    Debug.Log("找不到资源2"+assetname);
                }
                if (Callback != null)
                {
                    Callback();
                }
            });
        }

        /// <summary>
        /// 加载Sprite  图片名称[Sprite名称]
        /// </summary>
        /// <param name="image"></param>
        /// <param name="assetname"></param>
        public static void LoadSprite(SpriteRenderer spriteRenderer, string assetname)
        {
            CoreUtils.assetService.LoadAssetAsync<Sprite>(assetname, (asset) =>
            {
                if (asset == null)
                {
                    Debug.Log("找不到资源" + assetname);
                }
                var dat = asset.asset();
                if (dat != null && dat is Sprite)
                {
                    spriteRenderer.sprite = dat as Sprite;
                }
                else
                {
                    Debug.Log("找不到资源2" + assetname);
                }
            });
        }

        /// <summary>
        /// 替换 加载spine资源
        /// </summary>
        /// <param name="graphic"></param>
        /// <param name="assetname"></param>
        public static void LoadSpine(SkeletonGraphic graphic, string assetname, Action Callback = null)
        {
            CoreUtils.assetService.LoadAssetAsync<SkeletonDataAsset>(assetname, (asset) =>
            {
                if (asset == null)
                {
                    Debug.Log("找不到资源"+assetname);
                }
                var dat = asset.asset();
                if (dat!=null && dat is SkeletonDataAsset)
                {
                   var data = dat as SkeletonDataAsset;

                   ChangeSkeletonGraphic(graphic, data);
                }
                else
                {
                    Debug.Log("找不到资源2"+assetname);
                }
                if (Callback != null)
                {
                    Callback();
                }
            });
        }


        private static string[] mFormats = new[]
            {"{0}", "{1}", "{2}", "{3}", "{4}", "{5}", "{6}", "{7}", "{8}", "{9}", "{10}"};
        
        /// <summary>
        /// 策划配置的字符串 加 参数List<int>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string SafeFormat(string str, List<int> arr)
        {
            try
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (i<mFormats.Length)
                    {
                        str = str.Replace(mFormats[i], arr[i].ToString());
                    }
                }
                return str;
            }
            catch (Exception e)
            {
                Debug.Log(arr.Count+"格式化错误"+str+e.ToString());
            }

            return str;
        }
        /// <summary>
        /// 设置文本字体颜色#FFFFFFF
        /// </summary>
        /// <param name="text"></param>
        /// <param name="rgb"></param>
        public static void TextSetColor(Text text,string rgb)
        {
            
            Color c;
            ColorUtility.TryParseHtmlString(rgb, out c);
            text.color = c;
        }

        public static void ImageSetColor(PolygonImage image, string rgb)
        {
            Color c;
            ColorUtility.TryParseHtmlString(rgb, out c);
            image.color = c;
        }


        public static void ShowChild<T>(Transform node, int nCount) where T :Behaviour
        {
            for (int i = 0; i < node.childCount; i++)
            {
                var child = node.GetChild(i).GetComponent<T>();
                child.enabled = i < nCount;
            }
        }
    }
}
