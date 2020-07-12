using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class TankDefine
    {
        /// <summary> 
        /// 编号
        /// </summary>
        [PrimaryKey] [AutoIncrement] public int ID  { get; set; }

        /// <summary> 
        /// 最大血量
        /// </summary>
        public int HP;

        /// <summary> 
        ///  车前护甲值
        /// </summary>
        public int Armor_front;

        /// <summary> 
        ///  侧身护甲值 
        /// </summary>
        public int  Armor_side;

        /// <summary> 
        /// 背面护甲值
        /// </summary>
        public int  Armor_back;

        /// <summary> 
        /// 炮弹种类
        /// </summary>
        public string  Mag;

        /// <summary> 
        /// 坦克类型
        /// </summary>
        public string TankKind;

        /// <summary> 
        ///  坦克系列
        /// </summary>
        public string TankSerial;

        /// <summary> 
        ///  坦克速度
        /// </summary>
        public int Speed;

        /// <summary> 
        /// 道具携带数 
        /// </summary>
        public List<int> MaxItem;

        /// <summary> 
        ///  射击僵直
        /// </summary>
        public int ShootGap;

        /// <summary> 
        /// 移动僵直
        /// </summary>
        public int  MoveGap;

        /// <summary> 
        /// 装填时间
        /// </summary>
        public int ReloadGap;

    }
}