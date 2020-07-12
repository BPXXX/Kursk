//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
//using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
using System.Threading;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Reflection;

namespace Skyunion
{
    internal class HotfixApp_ILRT : HotfixApp
    {
        private AppDomain mApp;
        public HotfixApp_ILRT() { }
        ~HotfixApp_ILRT()
        {
            mApp = null;
        }

        public void Load(byte[] rDLLBytes, byte[] rPDBBytes)
        {
            MemoryStream rDLLMS = (rDLLBytes != null) ? new MemoryStream(rDLLBytes) : null;
            MemoryStream rPDBMS = (rPDBBytes != null) ? new MemoryStream(rPDBBytes) : null;
            Debug.Log("ILRT 热更新模式" + (rPDBMS != null ? " 附加调试信息" : " 没有调试信息"));
            mApp = new AppDomain();
            mApp.LoadAssembly (rDLLMS, rPDBMS, new Mono.Cecil.Pdb.PdbReaderProvider());

#if UNITY_EDITOR
            /*
            安装ILRuntime调试插件，并重新启动VS2015或VS2017
            运行Unity工程，并保证执行过appdomain.DebugService.StartDebugService(56000);来启动调试服务器
            用VisualStudio打开热更DLL项目
            点击菜单中的Debug->Attach to ILRuntime按钮
            在弹出来的窗口中填入被调试的主机的IP地址以及调试服务器的端口
            点击Attach按钮后，即可像UnityVS一样下断点调试
            */
            mApp.DebugService.StartDebugService (56000);
#if DEBUG
            //支持Profiler显示hotfix调用的内容
            mApp.UnityMainThreadID = Thread.CurrentThread.ManagedThreadId;
#endif
#endif
        }

        public IHotfixObject Instantiate (string rTypeName, params object[] rArgs) {
            if (mApp == null) return null;
            var rObject = new HotfixObject (this, rTypeName);
            rObject.mObject = mApp.Instantiate (rTypeName, rArgs);
            return rObject;
        }

        public T Instantiate<T> (string rTypeName, params object[] rObjs) {
            if (mApp == null) return default (T);
            return mApp.Instantiate<T> (rTypeName, rObjs);
        }

        public object Instantiate(Type type)
        {
            object obj = null;
            if (type is ILRuntimeType)
            {
                obj = ((ILRuntimeType)type).ILType.Instantiate().CLRInstance;
                //CoreUtils.logService.Debug($"ILRuntimeType:{type.ToString()}", Color.blue);
            }
            else
            {
                if (type is ILRuntimeWrapperType)
                    type = ((ILRuntimeWrapperType)type).RealType;
                obj = Activator.CreateInstance(type);
            }
            return obj;
        }

        public object Invoke (IHotfixObject rHotfixObj, string rMethodName, params object[] rArgs) {
            if (mApp == null || rHotfixObj == null) return null;
            return mApp.Invoke (rHotfixObj.TypeName(), rMethodName, rHotfixObj.Object(), rArgs);
        }

        public object InvokeParent (IHotfixObject rHotfixObj, string rParentType, string rMethodName, params object[] rArgs) {
            if (mApp == null || rHotfixObj == null) return null;
            return mApp.Invoke (rParentType, rMethodName, rHotfixObj.Object(), rArgs);
        }

        public object InvokeStatic (string rTypeName, string rMethodName, params object[] rArgs) {
            if (mApp == null) return null;
            return mApp.Invoke (rTypeName, rMethodName, null, rArgs);
        }

        public Type[] GetTypes () {
            if (mApp == null) return null;
            return mApp.LoadedTypes.Values.Select (x => x.ReflectionType).ToArray ();
        }
        
        public void printMemory()
        {
            List<TypeSizeInfo> detail;
            int size = mApp.GetSizeInMemory(out detail);
            
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(String.Format("当前IL内存占用{0:0.00} MB", size / 1024/1024));

            foreach (var dInfo in detail)
            {
                if (dInfo.TotalSize>5000)
                {
                    sb.AppendLine(String.Format("Type:\t{0}\t方法大小: {1:0.00} KB\t静态字段大小 {2:0.00} KB \t总大小 {3:0.00} KB",dInfo.Type.ToString(),dInfo.MethodBodySize/1024,dInfo.StaticFieldSize/1024,dInfo.TotalSize/1024));
                }
                
            }
            
            Debug.LogWarning(sb.ToString());
        }

        public void Close()
        {
#if UNITY_EDITOR 
            mApp.DebugService.StopDebugService();
#endif
        }
        public object GetAppDomain()
        {
            return mApp;
        }
    }
}