using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class AudioInfoDefine
    {
        /// <summary> 
        /// 声音事件ID
        /// </summary>
        [PrimaryKey] [AutoIncrement] public int ID  { get; set; }

        /// <summary> 
        /// 声音事件名称
        /// </summary>
        public string name;

        /// <summary> 
        /// 分组
        /// </summary>
        public int group;

        /// <summary> 
        /// 播放模式
        /// </summary>
        public int mode;

        /// <summary> 
        /// 同时播放数
        /// </summary>
        public int maxCount;

        /// <summary> 
        /// 同时播放上限处理
        /// </summary>
        public int maxCountAction;

        /// <summary> 
        /// 间隔时间
        /// </summary>
        public List<float> interval;

        /// <summary> 
        /// 音量
        /// </summary>
        public float volume;

        /// <summary> 
        /// 淡出时间
        /// </summary>
        public float fadeTime;

        /// <summary> 
        /// 3D音效
        /// </summary>
        public int config3D;

        /// <summary> 
        /// 音效文件
        /// </summary>
        public List<string> audios;

    }
}