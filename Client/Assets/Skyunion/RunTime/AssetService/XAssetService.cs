//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      资源加载服务类，提供文件的同步异步加载，提供Unity AB的同步异步加载和资源的声明周期。
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance

using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using log4net;
using log4net.Appender;
using log4net.Config;
using UnityEngine;
using UnityEngine.Networking;
using log4net.Util;
using System.Collections.Generic;
using Plugins.XAsset;
using UnityEngine.AddressableAssets;

namespace Skyunion
{
    class XAssetService : BaseAssetService
    {
        public override void BeforeInit()
        {
            mAssetService = mPluginManager.FindModule<IAssetService>();
        }

        public override void Init()
        {
            if (mAssetService != null)
            {
                Plugins.XAsset.Utility.loadFileDelegate = mAssetService.LoadFile;
                Plugins.XAsset.Utility.loadFileAsyncDelegate = mAssetService.LoadFileAsync;
            }
            Assets.Initialize(()=>
            {
                OnInitialized();
            }, 
            (error) => 
            {
                Debug.Log(error);
            });
        }
        public override IAsset LoadAssetAsync<T>(string assetName, Action<IAsset> completed)
        {
            var asset = Assets.LoadAsync(assetName, typeof(T));
            var newAsset = new XAsset(asset);
            asset.completed += delegate (Plugins.XAsset.Asset a)
            {
                completed(newAsset);
            };
            return newAsset;
        }
        public override IAsset LoadSceneAssetAsync(string assetName, bool addictive, Action<IAsset> completed)
        {
            var asset = Assets.LoadScene(assetName, true, addictive);
            var newAsset = new XAsset(asset);
            asset.completed += delegate (Plugins.XAsset.Asset a)
            {
                completed(newAsset);
            };
            return newAsset;
        }
        public override void Instantiate(string assetName, Action<GameObject> completed)
        {
            LoadAssetAsync<GameObject>(assetName, (IAsset asset) =>
            {
                var obj = GameObject.Instantiate(asset.asset()) as GameObject;
                asset.Attack(obj);
                completed(obj);
            });
        }
    }
}
