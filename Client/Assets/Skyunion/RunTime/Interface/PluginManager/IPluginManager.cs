//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      插件管理器接口
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance

using System;

namespace Skyunion
{
    public interface IPluginManager : IModule
    {
        //------------- 接口 -------------------//
        T FindModule<T>() where T : IModule;
        IModule FindModule(string strModuleName);
        void Registered(IPlugin plugin);
        void UnRegistered(IPlugin plugin);
        void AddModule(string strModuleName, IModule pModule);
        void RemoveModule(string strModuleName);
        void RemoveModule<T>() where T : IModule;

        Int64 GetInitTime();
        Int64 GetNowTime();
    };
}