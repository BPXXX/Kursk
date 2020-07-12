//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      游戏应用基类，初始化插件使用。
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance

using UnityEngine;
using System.Xml;
using System.Text;
using System;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Skyunion
{
    public class GameApp : MonoBehaviour
    {
        public bool mIsUsePluginConfig;
        public LogType logLevel = LogType.Exception;
        protected IPluginManager mPluginManager;
        protected virtual void OnAddPlugin()
        {
            PluginManager.Instance().Registered(new CorePlugin());
        }
        protected virtual void OnInitialized()
        {
        }
        protected virtual void OnAfterInitialized()
        {
        }

        private bool mCanUpdate = false;
        private float mStartTime;

        void Start()
        {
            logLevel = (LogType)PlayerPrefs.GetInt("LogType", (int)logLevel);
            Debug.unityLogger.filterLogType = logLevel;
            mPluginManager = PluginManager.Instance();

            mStartTime = Time.realtimeSinceStartup;
            DontDestroyOnLoad(gameObject);

            if (mIsUsePluginConfig)
            {
                CoreUtils.LoadFileAsync("Plugin.xml", (data) =>
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(Encoding.UTF8.GetString(data));

                    var appNode = xmlDocument.DocumentElement.ChildNodes[0];
                    var appName = xmlDocument.DocumentElement.GetAttribute("App");

#if UNITY_EDITOR
                    var mode = (HotfixMode)EditorPrefs.GetInt("HofixService_HofixMode", (int)HotfixMode.NativeCode);
                    if(mode == HotfixMode.ILRT)
                    {
                        appName = "ClientApp_ILRT";
                    }
                    else if (mode == HotfixMode.Reflect)
                    {
                        appName = "ClientApp_Reflect";
                    }
                    else if (mode == HotfixMode.NativeCode)
                    {
                        appName = "ClientApp_Native";
                    }
#endif

                    Debug.Log("App:"+appName);
                    foreach(var app in xmlDocument.DocumentElement.ChildNodes)
                    {
                        var appElement = app as XmlElement;
                        if (appElement.Name.Equals(appName))
                        {
                            appNode = appElement;
                            break;
                        }
                    }
                    var dynamicPlugin = new Plugin("DynamicPlugin");
                    mPluginManager.Registered(dynamicPlugin);
                    foreach(var childNode in appNode.ChildNodes)
                    {
                        var nodeElement = childNode as XmlElement;
                        if(nodeElement.Name.Equals("Module"))
                        {
                            var moduleName = nodeElement.GetAttribute("Name");
                            var moduleClass = nodeElement.GetAttribute("Class");
                            var assembly = nodeElement.GetAttribute("Assembly");
#if UNITY_EDITOR
                            if (moduleName.Equals("Skyunion.IDataService"))
                            {
                                var loadMode = EditorPrefs.GetInt("DataService_DataMode", (int)DataMode.Binary);
                                if (loadMode == (int)DataMode.Binary)
                                {
                                    moduleClass = "Skyunion.DataServiceBinary";
                                }
                                else if (loadMode == (int)DataMode.SQLite)
                                {
                                    moduleClass = "Skyunion.DataServiceSQLite";
                                }
                                else if (loadMode == (int)DataMode.Excel)
                                {
                                    moduleClass = "Skyunion.DataServiceExcel";
                                }
                            }
#endif
                            Debug.Log("module:" + moduleClass);
                            Type type;
                            if (assembly.Equals(string.Empty))
                            {
                                type = Type.GetType(moduleClass);
                            }
                            else
                            {
                                type = Assembly.Load(assembly).GetType(moduleClass);
                            }

                            IModule module = Activator.CreateInstance(type) as IModule;
                            dynamicPlugin.AddModule(moduleName, module);
                        }
                        else if (nodeElement.Name.Equals("Plugin"))
                        {
                            var pluginName = nodeElement.GetAttribute("Name");
                            var pluginClass = nodeElement.GetAttribute("Class");
                            var assembly = nodeElement.GetAttribute("Assembly");
                            Debug.Log("plugin:" + pluginClass);
                            Type type;
                            if (assembly.Equals(string.Empty))
                            {
                                type = Type.GetType(pluginClass);
                            }
                            else
                            {
                                type = Assembly.Load(assembly).GetType(pluginClass);
                            }
                            IPlugin plugin = Activator.CreateInstance(type) as IPlugin;
                            mPluginManager.Registered(plugin);
                        }
                    }
                    Init();
                });
                return;
            }

            OnAddPlugin();
        }

        private void Init()
        {
            mPluginManager.BeforeInit();
            mPluginManager.Init();
            mPluginManager.WaitInitAsync(() =>
            {
                OnInitialized();
                mPluginManager.AfterInit();
                OnAfterInitialized();
                CoreUtils.logService.Info(string.Format("total: {0} ms", Time.realtimeSinceStartup - mStartTime), Color.green);
                mCanUpdate = true;
            });
        }

        void Update()
        {
            if (mPluginManager != null)
            {
                mPluginManager.Update();
            }
        }
        void LateUpdate()
        {
            if (mPluginManager != null)
            {
                mPluginManager.LateUpdate();
            }
        }

        protected virtual void OnApplicationQuit()
        {
            if (mPluginManager != null)
            {
                mPluginManager.BeforeShut();
                mPluginManager.Shut();
                mPluginManager = null;
                mCanUpdate = false;
                CoreUtils.ClearCore();
            }
        }
    }
}