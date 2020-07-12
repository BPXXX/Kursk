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
    internal class HotfixObject :IHotfixObject
    {
        public string mTypeName;
        public HotfixApp mApp;
        public object mObject;

        public HotfixObject(HotfixApp rApp, string rTypeName)
        {
            this.mApp = rApp;
            this.mTypeName = rTypeName;
        }

        object IHotfixObject.Invoke(string rMethodName, params object[] rArgs)
        {
            if (this.mApp == null || this.mObject == null) return null;
            return this.mApp.Invoke(this, rMethodName, rArgs);
        }

        object IHotfixObject.InvokeParent(string rParentType, string rMethodName, params object[] rArgs)
        {
            if (this.mApp == null || this.mObject == null) return null;
            return this.mApp.InvokeParent(this, rParentType, rMethodName, rArgs);
        }

        object IHotfixObject.InvokeStatic(string rMethodName, params object[] rArgs)
        {
            if (this.mApp == null) return null;
            return this.mApp.InvokeStatic(this.mTypeName, rMethodName, null, rArgs);
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
}
