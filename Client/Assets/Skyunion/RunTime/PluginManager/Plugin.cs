//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      插件实现基类， 管理插件所支配的模块。
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Skyunion
{
    public class Plugin : Module, IPlugin
    {
        protected string mName;
        protected Dictionary<string, IModule> mModules = new Dictionary<string, IModule>();
        public void AddModule(string name, IModule module)
        {
            mPluginManager.AddModule(name, module);
            mModules.Add(name, module);
        }
        public void AddModule<T1>(IModule module)
        {
            string strName = typeof(T1).ToString();
            mPluginManager.AddModule(strName, module);
            mModules.Add(strName, module);
        }
        public void RemoveModule<T1>()
        {
            string strName = typeof(T1).ToString();
            IModule module;
            if (mModules.TryGetValue(strName, out module))
            {
                mPluginManager.RemoveModule(strName);
                mModules.Remove(strName);
            }
        }
        public Plugin(string name)
        {
            mName = name;
        }

        #region 实现 IModule
        public override void BeforeInit()
        {
            foreach (IModule module in mModules.Values)
            {
                if (module != null)
                {
                    module.BeforeInit();
                }
            }
        }

        public override void Init()
        {
            if(mModules.Count == 0)
            {
                OnInitialized();
                return;
            }
            foreach (IModule module in mModules.Values)
            {
                if (module != null)
                {
                    module.Init();
                    module.WaitInitAsync(() =>
                    {
                        mLoadedCount++;
                        if (mLoadedCount == mModules.Count)
                        {
                            OnInitialized();
                        }
                    });
                }
            }
        }

        public override void AfterInit()
        {
            foreach (IModule module in mModules.Values)
            {
                if (module != null)
                {
                    module.AfterInit();
                }
            }
        }

        public override void Update()
        {
            foreach (IModule module in mModules.Values)
            {
                if (module != null && module.Initialized())
                {
					module.Update();
                }
            }
        }

        public override void LateUpdate()
        {
            foreach (IModule module in mModules.Values)
            {
                if (module != null && module.Initialized())
                {
                    module.LateUpdate();
                }
            }
        }

        public override void BeforeShut()
        {
            foreach (IModule module in mModules.Values)
            {
                if (module != null)
                {
                    module.BeforeShut();
                }
            }
        }

        public override void Shut()
        {
            foreach (IModule module in mModules.Values)
            {
                if (module != null)
                {
                    module.Shut();
                }
            }
        }
        #endregion 实现 IModule

        #region 实现 IPlugin
        public virtual string GetPluginName()
        {
            return mName;
        }

        public virtual void OnAddModule()
        {
        }

        public void Install()
        {
            OnAddModule();
        }
        public void Uninstall()
        {
            foreach (var name in mModules.Keys)
            {
                mPluginManager.RemoveModule(name);
            }
            mModules.Clear();
        }
        #endregion 实现 IPlugin
    };
}