//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      插件管理器实现类，管理插件的整个声明周期。
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
    public class PluginManager : IModule, IPluginManager
    {
        private static IPluginManager _instance = null;
		public static IPluginManager Instance()
        {
            if (_instance == null)
            {
                _instance = new PluginManager();
            }
			return _instance;
        }

        protected bool mIsInitialized = false;
        protected int mLoadedCount = 0;
        private Action mCompleteAction;

        private Int64 mInitTime;
        private Int64 mNowTime;
        private Dictionary<string, IPlugin> mPlugins = new Dictionary<string, IPlugin>();
        private Dictionary<string, IModule> mModules = new Dictionary<string, IModule>();

        protected void OnInitialized()
        {
            mIsInitialized = true;
            mCompleteAction?.Invoke();
        }
        #region 实现 IModule
        public virtual bool Initialized()
        {
            return mIsInitialized;
        }

        public void BeforeInit()
        {
            foreach (IPlugin plugin in mPlugins.Values)
            {
                if (plugin != null)
                {
                    plugin.BeforeInit();
                }
            }
        }

        public void Init()
        {
            mInitTime = DateTime.Now.Ticks / 10000;
            foreach (IPlugin plugin in mPlugins.Values)
            {
                if (plugin != null)
                {
                    plugin.Init();
                    plugin.WaitInitAsync(() =>
                    {
                        mLoadedCount++;
                        if(mLoadedCount == mPlugins.Count)
                        {
                            OnInitialized();
                        }
                    });
                }
            }
        }

        public virtual void WaitInitAsync(Action complete)
        {
            mCompleteAction += complete;
            if (mIsInitialized)
            {
                complete();
            }
        }

        public void AfterInit()
        {
            foreach (IPlugin plugin in mPlugins.Values)
            {
                if (plugin != null)
                {
                    plugin.AfterInit();
                }
            }
        }
        public void Update()
        {
            mNowTime = DateTime.Now.Ticks / 10000;

            foreach (IPlugin plugin in mPlugins.Values)
            {
                if (plugin != null)
                {
					plugin.Update();
                }
            }
        }
        public void LateUpdate()
        {
            foreach (IPlugin plugin in mPlugins.Values)
            {
                if (plugin != null)
                {
                    plugin.LateUpdate();
                }
            }
        }

        public void BeforeShut()
        {
            foreach (IPlugin plugin in mPlugins.Values)
            {
                if (plugin != null)
                {
                    plugin.BeforeShut();
                }
            }
        }

        public void Shut()
        {
            foreach (IPlugin plugin in mPlugins.Values)
            {
                if (plugin != null)
                {
                    plugin.Shut();
                }
            }
        }
        #endregion 实现 IModule
        #region 实现 IPluginManager
        public T FindModule<T>() where T : IModule
        {
            IModule module = FindModule(typeof(T).ToString());

            return (T)module;
        }
        public IModule FindModule(string strModuleName)
        {
            IModule module;
            mModules.TryGetValue(strModuleName, out module);
            return module;
        }
        public void Registered(IPlugin plugin)
        {
            mPlugins.Add(plugin.GetPluginName(), plugin);
            plugin.Install();
        }
        public void UnRegistered(IPlugin plugin)
        {
            mPlugins.Remove(plugin.GetPluginName());
            plugin.Uninstall();
        }
        public void AddModule(string strModuleName, IModule pModule)
        {
            mModules.Add(strModuleName, pModule);
        }
        public void RemoveModule(string strModuleName)
        {
            mModules.Remove(strModuleName);
        }

		public void RemoveModule<T>() where T : IModule
        {
			RemoveModule(typeof(T).ToString());
        }
        public Int64 GetInitTime()
        {
            return mInitTime;
        }
        public Int64 GetNowTime()
        {
            return mNowTime;
        }
        #endregion 实现 IPluginManager
    };
}