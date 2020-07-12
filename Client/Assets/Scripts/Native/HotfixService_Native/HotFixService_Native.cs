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
using Skyunion;

public class HotFixService_Native : Module, IHotFixService
{
    private Hotfix.Game mGame;
    #region 实现 IModule    
    public override void AfterInit()
    {
        mGame = new Hotfix.Game();
        mGame.Initialize(mPluginManager, "HotFixService_Native");
    }

    public override void Update()
    {
        if (mGame != null)
        {
            mGame.Update();
        }
    }
    #endregion 实现 IModule

    #region 实现 HotFixService
    public HotfixMode GetHotfixMode()
    {
        return HotfixMode.NativeCode;
    }
    public IHotfixObject Instantiate(string rTypeName, params object[] rArgs)
    {
        var rObject = new HotfixObject_Native(rTypeName);
        rObject.mObject = Activator.CreateInstance(Type.GetType(rTypeName), rArgs);
        return rObject;
    }

    public T Instantiate<T>(string rTypeName, params object[] rObjs)
    {
        return (T)Activator.CreateInstance(Type.GetType(rTypeName), rObjs);
    }

    public T Instantiate<T>(Type type)
    {
        return (T)Instantiate(type);
    }

    public object Instantiate(Type type)
    {
        return Activator.CreateInstance(type);
    }

    public object Invoke(IHotfixObject rHotfixObj, string rMethodName, params object[] rArgs)
    {
        if (rHotfixObj == null) return null;
        return rHotfixObj.Invoke(rMethodName, rArgs);
    }

    public object InvokeParent(IHotfixObject rHotfixObj, string rParentType, string rMethodName, params object[] rArgs)
    {
        if (rHotfixObj == null) return null;
        return rHotfixObj.InvokeParent(rParentType, rMethodName, rArgs);
    }

    public object InvokeStatic(string rTypeName, string rMethodName, params object[] rArgs)
    {
        var rType = Type.GetType(rTypeName);
        var method = rType.GetMethod(rMethodName);
        return method.Invoke(null, rArgs);
    }
    public object GetAppdomain()
    {
        return null;
    }
    #endregion 实现 IHotFixService
}
