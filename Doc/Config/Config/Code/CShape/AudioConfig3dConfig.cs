using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class AudioConfig3dDefine
    {
        /// <summary> 
        /// 编号
        /// </summary>
        [PrimaryKey] [AutoIncrement] public int ID  { get; set; }

        /// <summary> 
        /// 组名称
        /// </summary>
        public string name;

        public float dopplerLevel;

        /// <summary> 
        /// 最小距离
        /// </summary>
        public float minDistance;

        /// <summary> 
        /// 最大距离
        /// </summary>
        public float maxDistance;

        public float spatialBlend;

        public int spread;

        public string curve;

    }
}