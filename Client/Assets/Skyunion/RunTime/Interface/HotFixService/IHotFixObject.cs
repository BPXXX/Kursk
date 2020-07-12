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

namespace Skyunion
{
    public interface IHotfixObject
    {
        string TypeName();
        object Object();
        object Invoke(string rMethodName, params object[] rArgs);
        object InvokeParent(string rParentType, string rMethodName, params object[] rArgs);
        object InvokeStatic(string rMethodName, params object[] rArgs);
    }
}
