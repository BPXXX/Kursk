using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class LanguageSetDefine
    {
        /// <summary> 
        /// 编号
        /// </summary>
        [PrimaryKey] [AutoIncrement] public int ID  { get; set; }

        /// <summary> 
        /// 本机语言代码
        /// </summary>
        public string language;

        /// <summary> 
        /// 语言包ID
        /// </summary>
        public int l_languageID;

        /// <summary> 
        /// 手机默认语言编号
        /// </summary>
        public List<int> telephone;

        /// <summary> 
        /// 语言开关,1=开启，0 =关闭
        /// </summary>
        public int enumSwitch;

        /// <summary> 
        /// 翻译语言代码
        /// </summary>
        public string translate;

        /// <summary> 
        /// 游戏版本号
        /// </summary>
        public string gameID;

    }
}