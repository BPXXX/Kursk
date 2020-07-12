//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      客户端模块的插件
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance

using Client;
using Skyunion;
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class ClientPlugin : Plugin
{
    public ClientPlugin() : base("ClientPlugin")
    {
    }

    public override void OnAddModule()
    {
    }
}