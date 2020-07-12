//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using System;
using UnityEngine;
using System.IO;
using System.Text;

namespace Skyunion
{ 
    internal interface HotfixApp
    {
        void Load(byte[] rDLLBytes, byte[] rPDBBytes);
        IHotfixObject Instantiate(string rTypeName, params object[] rArgs);
        T Instantiate<T>(string rTypeName, params object[] rArgs);
        object Instantiate(Type type);
        object Invoke(IHotfixObject rHotfixObj, string rMethodName, params object[] rArgs);
        object InvokeParent(IHotfixObject rHotfixObj, string rParentType, string rMethodName, params object[] rArgs);
        object InvokeStatic(string rTypeName, string rMethodName, params object[] rArgs);
        Type[] GetTypes();
        void printMemory();
        void Close();
        object GetAppDomain();
    }
}
