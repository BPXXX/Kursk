using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class MapDefine
    {
        /// <summary> 
        /// 编号
        /// </summary>
        [PrimaryKey] [AutoIncrement] public int ID  { get; set; }

        /// <summary> 
        /// 地图宽度
        /// </summary>
        public int Width;

        /// <summary> 
        /// 地图长度
        /// </summary>
        public int Length;

        /// <summary> 
        /// 客户端间距 
        /// </summary>
        public int ClientDistance;

        /// <summary> 
        /// 瓦片尺寸
        /// </summary>
        public int MapTileSize;

        /// <summary> 
        /// 据点
        /// </summary>
        public List<Point2> PointVec;

    }
}