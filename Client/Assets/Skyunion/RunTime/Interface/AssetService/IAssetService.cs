//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      资源服务 接口
//      android平台

//      路径属性    路径
//      Application.dataPath    /data/app/xxx.xxx.xxx.apk
//      Application.streamingAssetsPath jar:file:///data/app/xxx.xxx.xxx.apk/!/assets
//      Application.persistentDataPath  /data/data/xxx.xxx.xxx/files
//      Application.temporaryCachePath  /data/data/xxx.xxx.xxx/cache

//      ios平台

//      路径属性    路径
//      Application.dataPath    Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/xxx.app/Data
//      Application.streamingAssetsPath Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/xxx.app/Data/Raw
//      Application.persistentDataPath Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/Documents
//      Application.temporaryCachePath Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/Library/Caches
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance

using System;
using UnityEngine;

namespace Skyunion
{
   // 资源包括场景需要使用小写。尤其是场景，如果用大写后续会有问题（如果你传入读取场景是小写，实际是大写，会导致场景无法加载，目测是Unity的Bug）。
    public interface IAssetService : IModule
    {
        byte[] LoadFile(string path);
        void LoadFileAsync(string path, Action<byte[]> completed);
        IAsset LoadAssetAsync<T>(string assetName, Action<IAsset> completed) where T:UnityEngine.Object;
        IAsset LoadSceneAssetAsync(string assetName, bool addictive, Action<IAsset> completed);
        void Instantiate(string assetName, Action<GameObject> completed);
        void InstantiateOnePerFrame(string assetName, Action<GameObject> completed);
        void InstantiateSlowly(string assetName, Action<GameObject> completed);
        void Destroy(GameObject gameObject);
        void Destroy(GameObject gameObject, float fadeTime);
        GameObject Instantiate(GameObject gameObject);
    }
}
