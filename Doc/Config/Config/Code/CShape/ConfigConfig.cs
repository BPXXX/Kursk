using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class ConfigDefine
    {
        /// <summary> 
        /// 程序使用
        /// </summary>
        [PrimaryKey] [AutoIncrement] public int id  { get; set; }

        /// <summary> 
        /// 连接服务器的IP
        /// </summary>
        public string serverIP;

    }
}