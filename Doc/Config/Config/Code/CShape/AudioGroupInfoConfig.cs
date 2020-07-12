using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class AudioGroupInfoDefine
    {
        /// <summary> 
        /// 声音事件组ID
        /// </summary>
        [PrimaryKey] [AutoIncrement] public int ID  { get; set; }

        /// <summary> 
        /// 组名称
        /// </summary>
        public string name;

        /// <summary> 
        /// 音量
        /// </summary>
        public float baseVolume;

        /// <summary> 
        /// 同时播放数
        /// </summary>
        public int maxCount;

        /// <summary> 
        /// 同时播放上限处理
        /// </summary>
        public int maxCountAction;

        /// <summary> 
        /// 是否为背景音乐
        /// </summary>
        public int bgm;

        /// <summary> 
        /// 修改的音量组
        /// </summary>
        public List<int> modifyGroup;

        /// <summary> 
        /// 修正的音量
        /// </summary>
        public List<float> modifyVolume;

    }
}