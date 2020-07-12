//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      核心模块的插件
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Skyunion
{
    public class CorePlugin : Plugin
    {
        public CorePlugin():base("CorePlugin")
        {
        }
        public override void OnAddModule()
        {
            AddModule<ILogService>(new LogService());
            AddModule<INetServcice>(new NetService());
            //AddModule<IAssetService>(new XAssetService());
            AddModule<IAssetService>(new AddressAssetService());
            AddModule<IAudioService>(new AudioService());
            AddModule<IUIManager>(new UIManager());
            AddModule<IInputManager>(new InputManager());
            AddModule<IDataService>(new DataServiceSQLite());
            HotfixMode mode = HotfixMode.ILRT;
#if UNITY_EDITOR
            mode = (HotfixMode)EditorPrefs.GetInt("HofixService_HofixMode", (int)HotfixMode.NativeCode);
#endif
            if (mode != HotfixMode.NativeCode)
            {
                AddModule<IHotFixService>(new HotFixService());
            }
        }
    }
}
