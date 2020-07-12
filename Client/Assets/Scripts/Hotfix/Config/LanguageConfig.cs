using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class LanguageDefine
    {
        /// <summary> 
        /// 编号
        /// </summary>
        [PrimaryKey] [AutoIncrement] public int id  { get; set; }

        /// <summary> 
        /// 中文
        /// </summary>
        public string cn;

        /// <summary> 
        /// 英语
        /// </summary>
        public string en;

        /// <summary> 
        /// 阿语
        /// </summary>
        public string arabic;

        /// <summary> 
        /// 土耳其语
        /// </summary>
        public string tr;

    }
}