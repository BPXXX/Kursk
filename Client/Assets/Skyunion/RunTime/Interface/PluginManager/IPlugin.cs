//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      插件接口
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance

namespace Skyunion
{
    public interface IPlugin : IModule
    {
        //------------- 接口 -------------------//
        string GetPluginName();
        void Install();
        void Uninstall();
    };
}