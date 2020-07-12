//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      模块接口
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance

using System;

namespace Skyunion
{
    public interface IModule
    {
        //------------- 接口 -------------------//
        // 检测是否初始化完成
        bool Initialized();
        // 初始化前 准备一些变量初始化
        void BeforeInit();
        // 进行模块的初始化加载之类的
        void Init();
        // 等待初始化完成
        void WaitInitAsync(Action complete);
        // 全部初始化完进行操作
        void AfterInit();
        // 每帧更新
        void Update();
        // 每帧滞后更新
        void LateUpdate();
        // 关闭前
        void BeforeShut();
        // 关闭后
        void Shut();
    };
}