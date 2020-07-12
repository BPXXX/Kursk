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
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Plugins.XAsset;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement;

namespace Skyunion
{
    class AddressAssetService : BaseAssetService
    {
        public override void Init()
        {
            Addressables.InitializeAsync().Completed += AddressAssetService_Completed;
        }

        private void AddressAssetService_Completed(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<UnityEngine.AddressableAssets.ResourceLocators.IResourceLocator> obj)
        {
            mGameObject = new GameObject("AddressAssetService");
            mGameObject.AddComponent<ObjectPoolHandler>();
            GameObject.DontDestroyOnLoad(mGameObject);
            ResourceManager.ExceptionHandler = null;
            OnInitialized();
        }

        public override IAsset LoadAssetAsync<T>(string assetName, Action<IAsset> completed)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                Debug.LogErrorFormat("LoadAssetAsync param assetName is null");
                return null;
            }
            var handdler = Addressables.LoadAssetAsync<T>(assetName);
            if(handdler.IsDone && handdler.Result == null)
            {
                handdler = Addressables.LoadAssetAsync<T>("ErrorPrefab");
            }
            var newAsset = new AddressAsset<T>(handdler, assetName);
            handdler.Completed += (AsyncOperationHandle<T> obj)=>
            {
                mAssets.Enqueue(newAsset);
                LoadCallbackInUpdate(completed, newAsset);
            };
            return newAsset;
        }
        public override IAsset LoadSceneAssetAsync(string assetName, bool addictive, Action<IAsset> completed)
        {
            var handler = Addressables.LoadSceneAsync(assetName, addictive ? LoadSceneMode.Additive : LoadSceneMode.Single);
            var newAsset = new AddressSceneAsset(handler, assetName);
            handler.Completed += (AsyncOperationHandle<SceneInstance> obj) =>
            {
                completed?.Invoke(newAsset);
            };
            return newAsset;
        }
        public override void Instantiate(string assetName, Action<GameObject> completed)
        {
            // 已经知道是 回收对象了。直接调用回收池
            if (mObjectHasPool.ContainsKey(assetName))
            {
                ObjectPoolMgr.GetInstance().GetPool(assetName).Spawn(assetName, completed);
                return;
            }
            if(assetName == null || assetName.Equals(string.Empty))
            {
                Debug.LogError("param null:");
                return;
            }

            LoadAssetAsync<GameObject>(assetName, (IAsset asset) =>
            {
                GameObject gameObject;
                if (asset.asset() == null)
                {
                    gameObject = new GameObject(assetName);
                    completed?.Invoke(gameObject);
                    return;
                }
                else
                {
                    gameObject = asset.asset() as GameObject;
                }
                ObjectPoolItem component = gameObject.GetComponent<ObjectPoolItem>();
                if (component != null)
                {
                    if(component.poolName == null || component.poolName.Equals(string.Empty))
                    {
                        component.poolName = assetName;
                    }
                    ObjectPoolMgr.GetInstance().GetPool(component.poolName).Spawn(component.poolName, completed);
                }
                else
                {
                    var @object = CoreUtils.assetService.Instantiate(gameObject);
                    try
                    {
                        completed?.Invoke(@object);
                    }
                    catch(Exception ex)
                    {
                        Debug.LogError($"{assetName} Callback Error: {ex.ToString()}");
                    }
                }
            });
        }
        public override void Destroy(GameObject gameObject)
        {
            ObjectPoolItem component = gameObject.GetComponent<ObjectPoolItem>();
            if (component != null && component.poolName != null)
            {
                //Debug.Log($"{gameObject.name},{gameObject.GetInstanceID()}");
                ObjectPoolMgr.GetInstance().GetPool(component.poolName).DeSpawn(gameObject, component.poolName);
                if (!mObjectHasPool.ContainsKey(component.poolName))
                {
                    mObjectHasPool.Add(component.poolName, true);
                }
            }
            else
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }
        public override void Destroy(GameObject gameObject, float fadeTime)
        {
            // 临时不加延迟
            Destroy(gameObject);
        }

        protected override void UnloadUnusedAssets()
        {
            ObjectPoolMgr.GetInstance().TryCleanPool();
        }
    }
}
