using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class ItemDefine
    {
        /// <summary> 
        /// 编号
        /// </summary>
        [PrimaryKey] [AutoIncrement] public int id  { get; set; }

        /// <summary> 
        /// 道具类型
        /// </summary>
        public int type;

        /// <summary> 
        /// 道具名称
        /// </summary>
        public string name;

        /// <summary> 
        /// 道具品质
        /// </summary>
        public int quality;

    }
}