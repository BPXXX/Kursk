//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      日记服务实现类，管理日记的输出，使用log4net 可以自己配置日记输出形式。
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance

using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using log4net;
using log4net.Appender;
using log4net.Config;
using UnityEngine;
using UnityEngine.Networking;
using log4net.Util;
using System.Collections.Generic;

namespace Skyunion
{
    internal class LogService : Module, ILogService
    {
        private IAssetService mAssetService;
        private bool m_bConfigLoad = false;
        private bool m_bInUnityLog = false;
        private ILog m_unityLogger;
        private Dictionary<string, ILog> mDictoryLogger = new Dictionary<string, ILog>();
        private ILog GetLogger(string loggerName)
        {
            ILog logger;
            if (!mDictoryLogger.TryGetValue(loggerName, out logger))
            {
                logger = LogManager.GetLogger(loggerName);
                mDictoryLogger.Add(loggerName, logger);
            }
            return logger;
        }

        private void InitLog4Net()
        {
            // 可以自己修改日记的写入方式
            // http://logging.apache.org/log4net/release/config-examples.html
            string configPath = Path.Combine(Application.streamingAssetsPath, "log4net.xml");
            var byteData = mAssetService.LoadFile(configPath);
            if (byteData != null)
            {
                UnityEngine.Debug.Log($"load file {configPath} successed!");
                XmlDocument document = new XmlDocument();
                document.Load(new MemoryStream(byteData));
                XmlConfigurator.Configure(document.DocumentElement);
                m_unityLogger = LogManager.GetLogger("Unity");
                m_bConfigLoad = true;
            }
            else
            {
                UnityEngine.Debug.LogWarning($"load file {configPath} failure!");
            }
        }
        private void LogLog_LogReceived(object source, LogReceivedEventArgs e)
        {
            UnityEngine.Debug.Log(e.LogLog.Message);
            if (e.LogLog.Exception != null)
                UnityEngine.Debug.Log(e.LogLog.Exception.ToString());
        }
        private void ThreadLog(string condition, string stackTrace, LogType type)
        {
            if (m_bInUnityLog)
                return;

            m_bInUnityLog = true;
            if (!UnityEngine.Debug.unityLogger.IsLogTypeAllowed(type))
            {
                switch (type)
                {
                    case LogType.Error:
                        m_unityLogger.Error(condition + stackTrace);
                        break;
                    case LogType.Assert:
                        m_unityLogger.Warn(condition + stackTrace);
                        break;
                    case LogType.Warning:
                        m_unityLogger.Warn(condition + stackTrace);
                        break;
                    case LogType.Log:
                        m_unityLogger.Info(condition + stackTrace);
                        break;
                    case LogType.Exception:
                        m_unityLogger.Error(condition + stackTrace);
                        break;
                }
            }
            m_bInUnityLog = false;
        }

        #region 实现 IModule
        public override void BeforeInit()
        {
            mAssetService = mPluginManager.FindModule<IAssetService>();
#if !UNITY_EDITOR && UNITY_ANDROID
            //UnityEngine.Debug.unityLogger.filterLogType = LogType.Error;
#endif
        }

        public override void Init()
        {
            // 如果你的Log4Net无法正常工作，请取消一下注释
            //LogLog.InternalDebugging = true;
            //LogLog.QuietMode = false;
            //LogLog.LogReceived += LogLog_LogReceived;
            RollingFileAppender.SetLogFileDir(Application.temporaryCachePath);
            UnityEngine.Debug.Log("Log4netPath:" + Application.temporaryCachePath);
            InitLog4Net();

            if (m_bConfigLoad)
            {
                UnityEngine.Application.logMessageReceivedThreaded += ThreadLog;
            }
            OnInitialized();
        }


        public override void Shut()
        {
            if (m_bConfigLoad)
            {
                Application.logMessageReceivedThreaded -= ThreadLog;
            }
            LogManager.Shutdown();
        }
#endregion 实现 IModule

#region 实现 ILogService
        public void SetLogDir(string logDir)
        {
            UnityRollingFileAppender.SetLogFileDir(Application.temporaryCachePath);
            if (m_bConfigLoad)
                InitLog4Net();
        }
        public void Debug(string message)
        {
            if (!UnityEngine.Debug.unityLogger.IsLogTypeAllowed(LogType.Log))
                return;
            m_unityLogger.Debug(message);
            m_bInUnityLog = true;
            UnityEngine.Debug.Log(message);
            m_bInUnityLog = false;
        }
        public void Info(string message)
        {
            if (!UnityEngine.Debug.unityLogger.IsLogTypeAllowed(LogType.Log))
                return;
            m_unityLogger.Info(message);
            m_bInUnityLog = true;
            UnityEngine.Debug.Log(message);
            m_bInUnityLog = false;
        }
        public void Warn(string message)
        {
            if (!UnityEngine.Debug.unityLogger.IsLogTypeAllowed(LogType.Warning))
                return;
            m_unityLogger.Warn(message);
            m_bInUnityLog = true;
            UnityEngine.Debug.LogWarning(message);
            m_bInUnityLog = false;
        }
        public void Error(string message)
        {
            if (!UnityEngine.Debug.unityLogger.IsLogTypeAllowed(LogType.Error))
                return;
            m_unityLogger.Error(message);
            m_bInUnityLog = true;
            UnityEngine.Debug.LogError(message);
            m_bInUnityLog = false;
        }
        public void Fatal(string message)
        {
            if (!UnityEngine.Debug.unityLogger.IsLogTypeAllowed(LogType.Error))
                return;
            m_unityLogger.Error(message);
            m_bInUnityLog = true;
            UnityEngine.Debug.LogError(message);
            m_bInUnityLog = false;
        }
        public void Debug(string message, string loggerName)
        {
            if (!UnityEngine.Debug.unityLogger.IsLogTypeAllowed(LogType.Log))
                return;
            GetLogger(loggerName).Debug(message);
            m_bInUnityLog = true;
            UnityEngine.Debug.Log(message);
            m_bInUnityLog = false;
        }
        public void Info(string message, string loggerName)
        {
            if (!UnityEngine.Debug.unityLogger.IsLogTypeAllowed(LogType.Log))
                return;
            GetLogger(loggerName).Info(message);
            m_bInUnityLog = true;
            UnityEngine.Debug.Log(message);
            m_bInUnityLog = false;
        }
        public void Warn(string message, string loggerName)
        {
            if (!UnityEngine.Debug.unityLogger.IsLogTypeAllowed(LogType.Warning))
                return;
            GetLogger(loggerName).Warn(message);
            m_bInUnityLog = true;
            UnityEngine.Debug.LogWarning(message);
            m_bInUnityLog = false;
        }
        public void Error(string message, string loggerName)
        {
            if (!UnityEngine.Debug.unityLogger.IsLogTypeAllowed(LogType.Error))
                return;
            GetLogger(loggerName).Error(message);
            m_bInUnityLog = true;
            UnityEngine.Debug.LogError(message);
            m_bInUnityLog = false;
        }
        public void Fatal(string message, string loggerName)
        {
            if (!UnityEngine.Debug.unityLogger.IsLogTypeAllowed(LogType.Error))
                return;
            GetLogger(loggerName).Error(message);
            m_bInUnityLog = true;
            UnityEngine.Debug.LogError(message);
            m_bInUnityLog = false;
        }

        public void Debug(string message, Color color)
        {
            if (!UnityEngine.Debug.unityLogger.IsLogTypeAllowed(LogType.Log))
                return;
            m_unityLogger.Debug(message);
            m_bInUnityLog = true;
            message = string.Format("<b><color=#{0:X2}{1:X2}{2:X2}>{3}</color></b>\n", (byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), message);
            UnityEngine.Debug.Log(message);
            m_bInUnityLog = false;
        }
        public void Info(string message, Color color)
        {
            if (!UnityEngine.Debug.unityLogger.IsLogTypeAllowed(LogType.Log))
                return;
            m_unityLogger.Info(message);
            m_bInUnityLog = true;
            message = string.Format("<b><color=#{0:X2}{1:X2}{2:X2}>{3}</color></b>\n", (byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), message);
            UnityEngine.Debug.Log(message);
            m_bInUnityLog = false;
        }
        public void Warn(string message, Color color)
        {
            if (!UnityEngine.Debug.unityLogger.IsLogTypeAllowed(LogType.Warning))
                return;
            m_unityLogger.Warn(message);
            m_bInUnityLog = true;
            message = string.Format("<b><color=#{0:X2}{1:X2}{2:X2}>{3}</color></b>\n", (byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), message);
            UnityEngine.Debug.LogWarning(message);
            m_bInUnityLog = false;
        }
        public void Error(string message, Color color)
        {
            if (!UnityEngine.Debug.unityLogger.IsLogTypeAllowed(LogType.Error))
                return;
            m_unityLogger.Error(message);
            m_bInUnityLog = true;
            message = string.Format("<b><color=#{0:X2}{1:X2}{2:X2}>{3}</color></b>\n", (byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), message);
            UnityEngine.Debug.LogError(message);
            m_bInUnityLog = false;
        }
        public void Fatal(string message, Color color)
        {
            if (!UnityEngine.Debug.unityLogger.IsLogTypeAllowed(LogType.Error))
                return;
            m_unityLogger.Error(message);
            m_bInUnityLog = true;
            message = string.Format("<b><color=#{0:X2}{1:X2}{2:X2}>{3}</color></b>\n", (byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), message);
            UnityEngine.Debug.LogError(message);
            m_bInUnityLog = false;
        }
        public void Debug(string message, string loggerName, Color color)
        {
            if (!UnityEngine.Debug.unityLogger.IsLogTypeAllowed(LogType.Log))
                return;
            GetLogger(loggerName).Debug(message);
            m_bInUnityLog = true;
            message = string.Format("<b><color=#{0:X2}{1:X2}{2:X2}>{3}</color></b>\n", (byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), message);
            UnityEngine.Debug.Log(message);
            m_bInUnityLog = false;
        }
        public void Info(string message, string loggerName, Color color)
        {
            if (!UnityEngine.Debug.unityLogger.IsLogTypeAllowed(LogType.Log))
                return;
            GetLogger(loggerName).Info(message);
            m_bInUnityLog = true;
            message = string.Format("<b><color=#{0:X2}{1:X2}{2:X2}>{3}</color></b>\n", (byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), message);
            UnityEngine.Debug.Log(message);
            m_bInUnityLog = false;
        }
        public void Warn(string message, string loggerName, Color color)
        {
            if (!UnityEngine.Debug.unityLogger.IsLogTypeAllowed(LogType.Warning))
                return;
            GetLogger(loggerName).Warn(message);
            m_bInUnityLog = true;
            message = string.Format("<b><color=#{0:X2}{1:X2}{2:X2}>{3}</color></b>\n", (byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), message);
            UnityEngine.Debug.LogWarning(message);
            m_bInUnityLog = false;
        }
        public void Error(string message, string loggerName, Color color)
        {
            if (!UnityEngine.Debug.unityLogger.IsLogTypeAllowed(LogType.Error))
                return;
            GetLogger(loggerName).Error(message);
            m_bInUnityLog = true;
            message = string.Format("<b><color=#{0:X2}{1:X2}{2:X2}>{3}</color></b>\n", (byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), message);
            UnityEngine.Debug.LogError(message);
            m_bInUnityLog = false;
        }
        public void Fatal(string message, string loggerName, Color color)
        {
            if (!UnityEngine.Debug.unityLogger.IsLogTypeAllowed(LogType.Error))
                return;
            GetLogger(loggerName).Error(message);
            m_bInUnityLog = true;
            message = string.Format("<b><color=#{0:X2}{1:X2}{2:X2}>{3}</color></b>\n", (byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), message);
            UnityEngine.Debug.LogError(message);
            m_bInUnityLog = false;
        }
#endregion 实现 ILogService
    }
}
