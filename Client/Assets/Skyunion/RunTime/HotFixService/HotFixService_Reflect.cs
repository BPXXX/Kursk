//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      代码热更新服务类
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance

using System;
using System.Collections;
using System.IO;
using UnityEngine;
using log4net.Util;
using System.Collections.Generic;
using System.Threading;

namespace Skyunion
{
    internal class HotFixService_Reflect : HotFixService
    {
        public override HotfixMode HotfixDebugMode()
        {
            return HotfixMode.Reflect;
        }
    }
}
