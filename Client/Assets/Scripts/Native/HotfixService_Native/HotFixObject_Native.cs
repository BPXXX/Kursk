//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      热更新对象 接口
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance
using Skyunion;
using System;

class HotfixObject_Native : IHotfixObject
{
    public string mTypeName;
    public object mObject;
    public Type mType;

    public HotfixObject_Native(string rTypeName)
    {
        this.mTypeName = rTypeName;
        mType = Type.GetType(rTypeName);
    }

    object IHotfixObject.Invoke(string rMethodName, params object[] rArgs)
    {
        if (mObject == null)
            return null;
        var method = mType.GetMethod(rMethodName);
        return method.Invoke(mObject, rArgs);
    }

    object IHotfixObject.InvokeParent(string rParentType, string rMethodName, params object[] rArgs)
    {
        if (mObject == null)
            return null;
        var rType = Type.GetType(rParentType);
        var method = mType.GetMethod(rMethodName);
        return method.Invoke(mObject, rArgs);
    }

    object IHotfixObject.InvokeStatic(string rMethodName, params object[] rArgs)
    {
        var method = mType.GetMethod(rMethodName);
        return method.Invoke(null, rArgs);
    }

    string IHotfixObject.TypeName()
    {
        return mTypeName;
    }

    object IHotfixObject.Object()
    {
        return mObject;
    }
}