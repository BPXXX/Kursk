using UnityEngine;
using Skyunion;
using System.IO;
using UnityEngine.Profiling;
using System;
using Client;
using System.Collections.Generic;
using Data;
using Game;

namespace Hotfix
{
    public class Game
    {
        public void Initialize(IPluginManager pluginManager, string name)
        {
            AppFacade.GetInstance().StartUp();

            

            //CoreUtils.uiManager.ShowUI(UI.s_test,null);


           // CoreUtils.uiManager.ShowUI(UI.s_MainInterface, null);
            //    // 加载缓存的语言
            //    LanguageUtils.LoadCache();
            //    if (LanguageUtils.GetLanguage() == SystemLanguage.Unknown)
            //    {
            //        // 获取本机对应配置表的默认语言
            //        var defLanguage = GetDefaultLanguage();
            //        // 先设置为默认语言
            //        LanguageUtils.SetLanguage(defLanguage);
            //        // 弹出语言选择框
            //        int sureType = (int)UI_Win_LanguageMediator.SureType.DonotClose;
            //        sureType |= (int)UI_Win_LanguageMediator.SureType.OpenLoginView;
            //        sureType |= (int)UI_Win_LanguageMediator.SureType.AlwaysSave;
            //        CoreUtils.uiManager.ShowUI(UI.s_Pop_Language, null, sureType);
            //    }
            //    else
            //    {
            //        // 已经有默认语言直接进入登陆流程
            //        CoreUtils.uiManager.ShowUI(UI.s_LoadingView);
            //    }
        }
        private SystemLanguage GetDefaultLanguage()
        {
            int deviceLan = (int)Application.systemLanguage;
            var defaultLan = SystemLanguage.English;
            var langages = CoreUtils.dataService.QueryRecords<Data.LanguageSetDefine>();
            for (int i = 0; i < langages.Count; i++)
            {
                var lanConfig = langages[i];
                if (lanConfig.enumSwitch == 0)
                    continue;
                for (int j = 0; j < lanConfig.telephone.Count; j++)
                {
                    if (lanConfig.telephone[j] == (int)deviceLan)
                    {
                        return (SystemLanguage)lanConfig.ID;
                    }
                    if (lanConfig.telephone[j] == (int)SystemLanguage.Unknown)
                    {
                        defaultLan = (SystemLanguage)lanConfig.ID;
                    }
                }
            }

            return defaultLan;
        }
        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.F5))
            {
                //CoreUtils.uiManager.ShowUI(UI.s_gameTool);
            }
        }
    }
}
