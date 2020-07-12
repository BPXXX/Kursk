//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      本地代码插件
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance

using Skyunion;
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class NativePlugin : Plugin
{
    public NativePlugin() : base("NativePlugin")
    {
    }
    public override void OnAddModule()
    {
        HotfixMode mode = HotfixMode.ILRT;
#if UNITY_EDITOR
        mode = (HotfixMode)EditorPrefs.GetInt("HofixService_HofixMode", (int)HotfixMode.NativeCode);
#endif
        if (mode == HotfixMode.NativeCode)
        {
            AddModule<IHotFixService>(new HotFixService_Native());
        }
    }
}