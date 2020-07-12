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
using UnityEngine.SceneManagement;

namespace Skyunion
{
    class XAsset : IAsset
    {
        private Plugins.XAsset.Asset mAsset;
        public XAsset(Plugins.XAsset.Asset asset)
        {
            mAsset = asset;
        }

        public string assetName()
        {
            return mAsset.name;
        }
        public Scene scene()
        {
            return default(Scene);
        }

        public UnityEngine.Object asset()
        {
            return mAsset.asset;
        }

        public void Attack(UnityEngine.Object obj)
        {
            mAsset.Require(obj);
        }

        public void Release()
        {
            mAsset.Release();
        }
        public void Retain()
        {
            mAsset.Retain();
        }

        public bool Update()
        {
            return true;
        }
    }
}
