//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      代码热更新服务类
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance

using System;
using System.Collections;
using System.IO;
using UnityEngine;
using log4net.Util;
using System.Collections.Generic;
using System.Threading;

namespace Skyunion
{
    internal class HotFixService : Module, IHotFixService
    {
        private IAssetService mAssetService;
        private HotfixApp mApp;

        public HotFixService()
        {
            switch (HotfixDebugMode())
            {
                case HotfixMode.Reflect:
                    mApp = new HotfixApp_Reflect();
                    break;
                case HotfixMode.ILRT:
                    mApp = new HotfixApp_ILRT();
                    break;
                default:
                    mApp = new HotfixApp_ILRT();
                    break;
            }
        }

        public virtual HotfixMode HotfixDebugMode()
        {
            HotfixMode mode = HotfixMode.ILRT;
#if UNITY_EDITOR
            mode = (HotfixMode)UnityEditor.EditorPrefs.GetInt("HofixService_HofixMode", (int)HotfixMode.ILRT);
#endif
            return mode;
        }

        private IHotfixObject mHotFixMain;
        #region 实现 IModule
        public override void BeforeInit()
        {
            mAssetService = mPluginManager.FindModule<IAssetService>();
        }

        public override void Init()
        {
            mAssetService.WaitInitAsync(() =>
            {
                var rDLLBytes = mAssetService.LoadFile(Path.Combine(Application.streamingAssetsPath, "Hotfix.dll.bytes"));
                var rPDBBytes = mAssetService.LoadFile(Path.Combine(Application.streamingAssetsPath, "Hotfix.pdb.bytes"));
                mApp.Load(rDLLBytes, rPDBBytes);
                OnInitialized();
            });
        }
        public override void AfterInit()
        {
            mHotFixMain = Instantiate("Hotfix.Game");
            if (mHotFixMain != null)
            {
                mHotFixMain.Invoke("Initialize", mPluginManager, "HotFixService");
            }
        }

        public override void Update()
        {
            if (mHotFixMain != null)
            {
                mHotFixMain.Invoke("Update");
            }
        }

        public override void Shut()
        {
            mApp.Close();
        }
        #endregion 实现 IModule

        #region 实现 HotFixService
        public HotfixMode GetHotfixMode()
        {
            return HotfixDebugMode();
        }
        public IHotfixObject Instantiate(string rTypeName, params object[] rArgs)
        {
            return mApp.Instantiate(rTypeName, rArgs);
        }

        public T Instantiate<T>(string rTypeName, params object[] rObjs)
        {
            return mApp.Instantiate<T>(rTypeName, rObjs);
        }
        public T Instantiate<T>(Type type)
        {
            return (T)Instantiate(type);
        }
        public object Instantiate(Type type)
        {
            return mApp.Instantiate(type);
        }

        public object Invoke(IHotfixObject rHotfixObj, string rMethodName, params object[] rArgs)
        {
            if (rHotfixObj == null) return null;
            return this.mApp.Invoke(rHotfixObj, rMethodName, rArgs);
        }

        public object InvokeParent(IHotfixObject rHotfixObj, string rParentType, string rMethodName, params object[] rArgs)
        {
            if (rHotfixObj == null) return null;
            return this.mApp.InvokeParent(rHotfixObj, rParentType, rMethodName, rArgs);
        }

        public object InvokeStatic(string rTypeName, string rMethodName, params object[] rArgs)
        {
            return mApp.InvokeStatic(rTypeName, rMethodName, null, rArgs);
        }
        public object GetAppdomain()
        {
            return mApp.GetAppDomain();
        }
        #endregion 实现 IHotFixService
    }
}
