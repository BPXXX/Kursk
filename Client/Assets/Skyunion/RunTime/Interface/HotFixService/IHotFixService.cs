//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      热更新 服务接口
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance

using System;

namespace Skyunion
{
    public enum HotfixMode
    {
        Reflect,
        ILRT,
        NativeCode,
    }
    public interface IHotFixService : IModule
    {
        HotfixMode GetHotfixMode();
        IHotfixObject Instantiate(string rTypeName, params object[] rArgs);
        T Instantiate<T>(string rTypeName, params object[] rArgs);
        T Instantiate<T>(Type type);
        object Instantiate(Type type);
        object Invoke(IHotfixObject rHotfixObj, string rMethodName, params object[] rArgs);
        object InvokeParent(IHotfixObject rHotfixObj, string rParentType, string rMethodName, params object[] rArgs);
        object InvokeStatic(string rTypeName, string rMethodName, params object[] rArgs);
        object GetAppdomain();
    }
}
