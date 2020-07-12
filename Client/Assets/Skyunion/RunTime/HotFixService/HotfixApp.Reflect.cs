//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Skyunion
{
    internal class HotfixApp_Reflect : HotfixApp
    {
        private Assembly mApp;
        
        public void Load(byte[] rDLLBytes, byte[] rPDBBytes)
        {
            Debug.Log("Reflect 热更新模式"+ (rPDBBytes != null ? " 附加调试信息" : " 没有调试信息"));
            mApp = Assembly.Load(rDLLBytes, rPDBBytes);
        }

        public IHotfixObject Instantiate(string rTypeName, params object[] rArgs)
        {
            if (mApp == null) return null;
            var rObject = new HotfixObject(this, rTypeName);
            rObject.mObject = Activator.CreateInstance(mApp.GetType(rTypeName), rArgs);
            return rObject;
        }

        public T Instantiate<T>(string rTypeName, params object[] rArgs)
        {
            if (mApp == null) return default(T);
            return (T)Activator.CreateInstance(mApp.GetType(rTypeName), rArgs);
        }

        public object Instantiate(Type type)
        {
            return Activator.CreateInstance(type);
        }

        public object Invoke(IHotfixObject rHotfixObj, string rMethodName, params object[] rArgs)
        {
            if (mApp == null) return null;
            Type rObjType = mApp.GetType(rHotfixObj.TypeName());
            return ReflectionAssist.MethodMember(rHotfixObj.Object(), rMethodName, ReflectionAssist.flags_method_inst, rArgs);
        }

        public object InvokeParent(IHotfixObject rHotfixObj, string rParentType, string rMethodName, params object[] rArgs)
        {
            if (mApp == null) return null;
            Type rObjType = mApp.GetType(rParentType);
            return ReflectionAssist.MethodMember(rHotfixObj.Object(), rMethodName, ReflectionAssist.flags_method_inst, rArgs);
        }

        public object InvokeStatic(string rTypeName, string rMethodName, params object[] rArgs)
        {
            if (mApp == null) return null;
            Type rObjType = mApp.GetType(rTypeName);
            return rObjType.InvokeMember(rMethodName, ReflectionAssist.flags_method | BindingFlags.Static, null, null, rArgs);
        }

        public Type[] GetTypes()
        {
            if (mApp == null) return null;
            return mApp.GetTypes();
        }

        public void Close()
        {
        }

        public void printMemory()
        {
        }
        public object GetAppDomain()
        {
            return null;
        }
    }
}
