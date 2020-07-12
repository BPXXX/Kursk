//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      资源实现类，对 XAsset的 Asset进行绑定。
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
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Skyunion
{
    class AddressAsset<T> : IAsset where T :UnityEngine.Object
    {
        private AsyncOperationHandle<T> mHanddler;
        private Queue<UnityEngine.Object> mAttackObjects = new Queue<UnityEngine.Object>();
        int mRefCount = 0;
        private string mAssetName;
        public AddressAsset(AsyncOperationHandle<T> handdler, string name)
        {
            mHanddler = handdler;
            mAssetName = name;
        }

        public string assetName()
        {
            return mAssetName;
        }
        public UnityEngine.Object asset()
        {
            return mHanddler.Result;
        }
        public Scene scene()
        {
            return default(Scene);
        }

        public void Attack(UnityEngine.Object obj)
        {
            mAttackObjects.Enqueue(obj);
            Retain();
        }

        public void Release()
        {
            mRefCount--;
        }

        public void Retain()
        {
            mRefCount++;
        }

        public bool Update()
        {
            if(mAttackObjects.Count > 0)
            {
                if(mAttackObjects.Peek() == null)
                {
                    mAttackObjects.Dequeue();
                    Release();
                }
            }
            if(mRefCount == 0)
            {
                Addressables.Release(mHanddler);
                return true;
            }
            return false;
        }
    }
    class AddressSceneAsset : IAsset
    {
        private AsyncOperationHandle<SceneInstance> mHanddler;
        private Queue<UnityEngine.Object> mAttackObjects = new Queue<UnityEngine.Object>();
        int mRefCount = 0;
        private string mAssetName;
        public AddressSceneAsset(AsyncOperationHandle<SceneInstance> handdler, string name)
        {
            mHanddler = handdler;
            mAssetName = name;
        }

        public string assetName()
        {
            return mAssetName;
        }
        public Scene scene()
        {
            return mHanddler.Result.Scene;
        }
        public UnityEngine.Object asset()
        {
            return null;
        }

        public void Attack(UnityEngine.Object obj)
        {
            mAttackObjects.Enqueue(obj);
            Retain();
        }

        public void Release()
        {
            mRefCount--;
        }

        public void Retain()
        {
            mRefCount++;
        }

        public bool Update()
        {
            if (mAttackObjects.Count > 0)
            {
                if (mAttackObjects.Peek() == null)
                {
                    mAttackObjects.Dequeue();
                    Release();
                }
            }
            if (mRefCount == 0)
            {
                Addressables.Release(mHanddler);
                return true;
            }
            return false;
        }
    }
}
