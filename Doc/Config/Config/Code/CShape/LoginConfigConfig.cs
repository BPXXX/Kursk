using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class LoginConfigDefine
    {
        /// <summary> 
        /// 程序使用
        /// </summary>
        [PrimaryKey] [AutoIncrement] public int id  { get; set; }

        /// <summary> 
        /// 创角登录服务器IP
        /// </summary>
        public string serverIP;

        /// <summary> 
        /// 创角登录用于初始创角登陆,代表轻型坦克的血量
        /// </summary>
        public int health_lite;

        /// <summary> 
        /// 创角登录用于初始创角登陆，代表中型坦克的血量
        /// </summary>
        public int health_mid;

        /// <summary> 
        /// 创角登录用于初始创角登陆，代表重型坦克的血量
        /// </summary>
        public int health_heavy;

        /// <summary> 
        /// 创角登录用于初始创角登陆，轻型坦克的装填时间
        /// </summary>
        public int load_lite;

        /// <summary> 
        /// 创角登录用于初始创角登陆，中型坦克的装填时间
        /// </summary>
        public int load_mid;

        /// <summary> 
        /// 创角登录用于初始创角登陆，重型坦克的装填时间
        /// </summary>
        public int load_heavy;

    }
}