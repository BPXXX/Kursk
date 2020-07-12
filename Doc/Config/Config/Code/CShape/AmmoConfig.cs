using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class AmmoDefine
    {
        /// <summary> 
        /// 编号
        /// </summary>
        [PrimaryKey] [AutoIncrement] public int ID  { get; set; }

        /// <summary> 
        /// 威力
        /// </summary>
        public int Damage;

        /// <summary> 
        /// 穿甲
        /// </summary>
        public int ArmorPiercing;

    }
}