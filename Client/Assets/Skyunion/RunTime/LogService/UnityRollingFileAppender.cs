//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      Log4net Unity输出插件 主要用来修改日记生成的位置
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
using log4net.Core;
using UnityEngine;
using UnityEngine.Networking;

namespace Skyunion
{
    public class UnityRollingFileAppender : RollingFileAppender
    {
        //private static string m_strLogFileDir = "";
        //public static void SetLogFileDir(string dir)
        //{
        //    m_strLogFileDir = dir;
        //}
        //public override void ActivateOptions()
        //{
        //    if(m_strLogFileDir.Equals(string.Empty))
        //    {
        //        m_strLogFileDir = Application.temporaryCachePath;
        //    }
        //    File = Path.Combine(m_strLogFileDir, File);
        //    base.ActivateOptions();
        //    Debug.Log("[logfilePath]:" + File);
        //}
        public override void ActivateOptions()
        {
            base.ActivateOptions();
            Debug.Log("[logfilePath]:" + File);
        }
    }
}
