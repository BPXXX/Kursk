//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      日记服务接口
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance

using UnityEngine;

namespace Skyunion
{
    public interface ILogService : IModule
    {
        void SetLogDir(string logDir);
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Fatal(string message);
        void Debug(string message, string loggerName);
        void Info(string message, string loggerName);
        void Warn(string message, string loggerName);
        void Error(string message, string loggerName);
        void Fatal(string message, string loggerName);
        void Debug(string message, Color color);
        void Info(string message, Color color);
        void Warn(string message, Color color);
        void Error(string message, Color color);
        void Fatal(string message, Color color);
        void Debug(string message, string loggerName, Color color);
        void Info(string message, string loggerName, Color color);
        void Warn(string message, string loggerName, Color color);
        void Error(string message, string loggerName, Color color);
        void Fatal(string message, string loggerName, Color color);
    }
}
