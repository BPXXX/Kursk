using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILRuntime.Reflection;
using Skyunion;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Skyunion
{
    internal class LanguageDefine
    {
        /// <summary> 
        /// 编号
        /// </summary>
        [PrimaryKey] [AutoIncrement] public int id { get; set; }

        /// <summary> 
        /// 中文
        /// </summary>
        public string cn = string.Empty;

        /// <summary> 
        /// 英语
        /// </summary>
        public string en = string.Empty;

        /// <summary> 
        /// 阿语
        /// </summary>
        public string arabic = string.Empty;

        /// <summary> 
        /// 土耳其语
        /// </summary>
        public string tr = string.Empty;

    }
    public static class LanguageUtils
    {
        private static SystemLanguage language = SystemLanguage.Unknown;
        public static SystemLanguage GetLanguage()
        {
            if(language == SystemLanguage.Unknown)
            {
                var cur_language = PlayerPrefs.GetInt("CurrentLanguage", (int)SystemLanguage.Unknown);
                language = (SystemLanguage)cur_language;
            }
            return language;
        }

        public static void SetLanguage(SystemLanguage value)
        {
            language = value;
            var uiRoot = GameObject.Find("ClientApp/UIRoot");
            if (uiRoot != null)
            {
                var lanTexts = uiRoot.GetComponentsInChildren<LanguageText>();
                foreach (var text in lanTexts)
                {
                    text.UpdateLanguage();
                    text.SetAllDirty();
                }
            }
            else
            {
                var lanTexts = GameObject.FindObjectsOfType<LanguageText>();
                foreach (var text in lanTexts)
                {
                    text.UpdateLanguage();
                    text.SetAllDirty();
                }
            }
        }
        public static void SaveCache()
        {
            PlayerPrefs.SetInt("CurrentLanguage", (int)language);
        }
        public static void LoadCache()
        {
            var cur_language = (SystemLanguage)PlayerPrefs.GetInt("CurrentLanguage", (int)SystemLanguage.Unknown);
            SetLanguage(cur_language);
        }
        public static void ClearCache()
        {
            PlayerPrefs.DeleteKey("CurrentLanguage");
        }
        public static bool IsArabic()
        {
            return language == SystemLanguage.Arabic;
        }

        private static TableBinary<LanguageDefine> mLanguageTable = null;
        public static void ReloadConfig()
        {
            mLanguageTable = null;
        }
        public static string getText(int id)
        {
            LanguageDefine lan = null;
#if UNITY_EDITOR
            if (Application.isPlaying && CoreUtils.dataService != null)
            {
                lan = CoreUtils.dataService.QueryRecord<LanguageDefine>(id);
            }
            else
            {
                if (mLanguageTable == null)
                {
                    mLanguageTable = new TableBinary<LanguageDefine>();
                }
                lan = mLanguageTable.QueryRecord(id);
            }
#else
            lan = CoreUtils.dataService.QueryRecord<LanguageDefine>(id);
#endif

            if (lan == null)
            {
                return "Not Found:" + id;
            }

            switch (language)
            {
                case SystemLanguage.ChineseSimplified:
                case SystemLanguage.Chinese:
                case SystemLanguage.ChineseTraditional:
                    return lan.cn;
                case SystemLanguage.English:
                    return lan.en;
                case SystemLanguage.Arabic:
                    return lan.arabic;
                default:
                    return lan.en;
            }
        }

        /// <summary>
        /// 获取当前语言的ID文本并格式化
        /// </summary>
        /// <returns>The text format.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="arg">Argument.</param>
        public static string getTextFormat(int id, params object[] arg)
        {
            string lan = getText(id);
            string str;
            try
            {
                str = string.Format(lan, arg);
            }
            catch (Exception e)
            {
                str = lan;
                Debug.LogErrorFormat(" {0}:{1} 语言包格式错误 ", id, lan);
            }

            return str;
        }
    }
}
