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
    abstract class BaseAssetService : Module, IAssetService
    {
        protected IAssetService mAssetService;
        protected GameObject mGameObject;
        protected Dictionary<string, bool> mObjectHasPool = new Dictionary<string, bool>();
        protected Queue<IAsset> mAssets = new Queue<IAsset>();
        protected struct InstanceRequest
        {
            public Action<GameObject> callback;
            public string assetName;
        }
        protected struct LoadedCallback
        {
            public Action<IAsset> callback;
            public IAsset asset;
        }
        private Queue<InstanceRequest> mInstanceOnePerFrameQueue = new Queue<InstanceRequest>();
        private Queue<InstanceRequest> mInstanceSlowlyQueue = new Queue<InstanceRequest>();
        private Queue<LoadedCallback> mLoadAssetQueue = new Queue<LoadedCallback>();

        public float m_unload_unused_asset_time = 60f;
        private float m_unload_unused_asset_timer;
        public override void Update()
        {
            //if(mAssets.Count > 0)
            //{
            //    if(mAssets.Peek().Update())
            //    {
            //        mAssets.Dequeue();
            //    }
            //}
            int nCount = 0;
            while(mInstanceSlowlyQueue.Count > 0 && nCount != 20)
            {
                var request = mInstanceSlowlyQueue.Dequeue();
                if (request.callback != null)
                {
                    this.Instantiate(request.assetName, request.callback);
                }
                nCount++;
            }

            if(mInstanceOnePerFrameQueue.Count > 0)
            {
                var request = mInstanceOnePerFrameQueue.Dequeue();
                if (request.callback != null)
                {
                    this.Instantiate(request.assetName, request.callback);
                }
            }
            while (mLoadAssetQueue.Count > 0)
            {
                var respon = mLoadAssetQueue.Dequeue();
                respon.callback?.Invoke(respon.asset);
            }

            m_unload_unused_asset_timer += Time.deltaTime;
            if (m_unload_unused_asset_timer >= m_unload_unused_asset_time)
            {
                UnloadUnusedAssets();
                m_unload_unused_asset_timer = 0f;
            }
        }

        public byte[] LoadFile(string path)
        {
            if (path.IndexOf(Path.PathSeparator) == -1)
            {
                path = Path.Combine(Application.streamingAssetsPath, path);
            }
#if UNITY_ANDROID && !UNITY_EDITOR
            if(path.StartsWith("jar:"))
            {
                path = path.Substring(Application.streamingAssetsPath.Length+1);
                Debug.Log("new File Path:"+ path);
                AndroidJavaClass m_AndroidJavaClass = new AndroidJavaClass("com.unity.androidplugin.AssetLoad");
                var sdata = m_AndroidJavaClass.CallStatic<sbyte[]>("loadFile", path);
                var signed = new byte[sdata.Length];
                Buffer.BlockCopy(sdata, 0, signed, 0, sdata.Length);
                return signed;
            }
#endif

            return File.ReadAllBytes(path);
        }        

        public void LoadFileAsync(string path, Action<byte[]> completed)
        {
            if (path.IndexOf(Path.PathSeparator) == -1)
            {
                path = Path.Combine(Application.streamingAssetsPath, path);
            }
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            path = path.Insert(0, "file://");
#endif
            
            UnityWebRequest webFile = UnityWebRequest.Get(path);
            webFile.SendWebRequest().completed += (AsyncOperation op) =>
            {
                completed?.Invoke(webFile.downloadHandler.data);
            };
        }
        public void InstantiateOnePerFrame(string assetName, Action<GameObject> completed)
        {
            var request = new InstanceRequest();
            request.assetName = assetName;
            request.callback = completed;
            mInstanceOnePerFrameQueue.Enqueue(request);
        }
        public void InstantiateSlowly(string assetName, Action<GameObject> completed)
        {
            var request = new InstanceRequest();
            request.assetName = assetName;
            request.callback = completed;
            mInstanceSlowlyQueue.Enqueue(request);
        }
        protected void LoadCallbackInUpdate(Action<IAsset> completed, IAsset asset)
        {
            var respon = new LoadedCallback();
            respon.asset = asset;
            respon.callback = completed;
            mLoadAssetQueue.Enqueue(respon);
        }

        public abstract IAsset LoadAssetAsync<T>(string assetName, Action<IAsset> completed) where T : UnityEngine.Object;
        public abstract IAsset LoadSceneAssetAsync(string assetName, bool addictive, Action<IAsset> completed);
        public abstract void Instantiate(string assetName, Action<GameObject> completed);

        public virtual  void Destroy(GameObject gameObject)
        {
            GameObject.Destroy(gameObject);
        }
        public virtual void Destroy(GameObject gameObject, float fadeTime)
        {
            GameObject.Destroy(gameObject, fadeTime);
        }

        protected virtual void UnloadUnusedAssets()
        {
        }
        public GameObject Instantiate(GameObject gameObject)
        {
            return GameObject.Instantiate(gameObject);
        }
    }
}
