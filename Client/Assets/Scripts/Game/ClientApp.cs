//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      客户端应用类，初始化插件使用。
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance

using UnityEngine;
using System.Collections;
using Skyunion;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
using System;
using System.Reflection;
using Client;

public class ClientApp : GameApp
{
    public GameObject m_SceneObject;

    private void Awake()
    {
        if (m_SceneObject)
        {
            m_SceneObject.SetActive(false);
        }
    }

    // 添加客户端自己的插件
    protected override void OnAddPlugin()
    {
        mPluginManager.Registered(new CorePlugin());
        mPluginManager.Registered(new ClientPlugin());
        mPluginManager.Registered(new NativePlugin());
    }

    // 所有插件初始化调用
    protected override void OnInitialized()
    {
        if (CoreUtils.hotService.GetHotfixMode() == HotfixMode.ILRT)
        {
            AppDomain app = CoreUtils.hotService.GetAppdomain() as AppDomain;
            ILRTBind.ILRTBind.Init(app);
        }
        if (m_SceneObject)
        {
            m_SceneObject.SetActive(true);
        }
        LanguageUtils.SetLanguage(LanguageUtils.GetLanguage());
    }

    protected override void OnAfterInitialized()
    {
        CoreUtils.logService.Debug(string.Format("Current Hotfix Mode:{0}", CoreUtils.hotService.GetHotfixMode().ToString()), Color.green);
        CoreUtils.logService.Debug(string.Format("Current Data Mode:{0}", CoreUtils.dataService.GetDataMode().ToString()), Color.green);
    }
    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        ClientUtils.ClearCore();
    }
}