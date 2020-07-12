// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月30日
// Update Time         :    2019年12月30日
// Class Description   :    GameToolMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using SprotoType;
using UnityEngine.UI;

namespace Game {
    public class GameToolMediator : GameMediator {
        #region Member
        public static string NameMediator = "GameToolMediator";

        #endregion

        //IMediatorPlug needs
        public GameToolMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public GameToolView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                default:
                    break;
            }
        }

       

        #region UI template method

        public override void OpenAniEnd(){

        }

        public override void WinFocus(){
            
        }

        public override void WinClose(){
            
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
        }

        protected override void BindUIEvent()
        {
            view.m_ls_template_ButtonAnimation.gameObject.SetActive(false);
        }

        protected override void BindUIData()
        {
            view.m_btn_tip_GameButton.onClick.AddListener(()=>
            {
                HelpTip.CreateTip("<size=50>测试常驻的Tip</size>", view.m_btn_tip_GameButton.transform).SetStyle(HelpTipData.Style.arrowUp).SetOffset(20f).SetWidth(200f).Show();
            });
            AddMenu("测试Tip", () =>
            {
                Tip.CreateTip("显示tip").Show();
            });
            AddMenu("测试Tip2", () =>
            {
                HelpTip.CreateTip("<size=50>测试常驻的Tip</size>", view.m_btn_tip_GameButton.transform).SetStyle(HelpTipData.Style.arrowUp).SetOffset(20f).SetWidth(200f).Show();
            });
            AddMenu("播放BGM1", () =>
            {
                CoreUtils.audioService.PlayBgm("bgm_env_day");
            });
            AddMenu("播放BGM2", () =>
            {
                CoreUtils.audioService.PlayBgm("bgm_env_night");
            });
            AddMenu("播放随机音效", () =>
            {
                CoreUtils.audioService.PlayBgm("sfx_env_day");
            });
            AddMenu("播放欢迎语", () =>
            {
                CoreUtils.audioService.PlayOneShot("vo_welcome_desc", null);
            });
            AddMenu("测试Alert", () =>
            {
                Alert.CreateAlert("测试", "测试").SetLeftButton().SetRightButton(() => { Tip.CreateTip("显示tip").Show(); }).Show();
            });
            AddMenu("切换语言", () =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_LoadingView);
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
                int sureType = (int)UI_Win_LanguageMediator.SureType.CloseAll;
                sureType |= (int)UI_Win_LanguageMediator.SureType.OpenLoginView;
                sureType |= (int)UI_Win_LanguageMediator.SureType.ResetScene;
                CoreUtils.uiManager.ShowUI(UI.s_Pop_Language, null, sureType);
            });
            AddMenu("清楚缓存", () =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
                LanguageUtils.ClearCache();
            });
        }

        #endregion

        private void AddMenu(string name, UnityEngine.Events.UnityAction action)
        {
            var menu = Object.Instantiate(view.m_ls_template_ButtonAnimation.gameObject, view.m_ls_template_ButtonAnimation.transform.parent);
            menu.SetActive(true);
            menu.GetComponent<GameButton>().onClick.AddListener(action);
            menu.GetComponentInChildren<Text>().text = name;
        }
    }
}