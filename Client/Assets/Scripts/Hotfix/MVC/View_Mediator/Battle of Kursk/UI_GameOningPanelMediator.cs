// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月3日
// Update Time         :    2020年6月3日
// Class Description   :    UI_GameOningPanelMediator
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
using System;

namespace Game {
    public class UI_GameOningPanelMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_GameOningPanelMediator";


        #endregion

        //IMediatorPlug needs
        public UI_GameOningPanelMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_GameOningPanelView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.SingleName,
                CmdConstant.TatalScores,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.SingleName:
                    {
                        view.m_lbl_languageText_LanguageText.text = TmpData.userInfoSelf.user_name;
                        if(TmpData.userInfoSelf.position_number<7)
                        {
                            ClientUtils.LoadSprite(view.m_img_polygonImage_PolygonImage, "Head_Blue[head1]");
                        }
                        else
                        {
                            ClientUtils.LoadSprite(view.m_img_polygonImage_PolygonImage, "Head_Red[head2]");
                        }
                    }
                    break;
                case CmdConstant.TatalScores:
                    {
                        view.m_lbl_SingleScore_LanguageText.text = TmpData.userInfoSelf.user_point.ToString();
                        ShowScores();
                    }
                    break;
            }
        }

        private void ShowScores()
        {
            int Score_red=0, Score_Blue=0;
            int count = TmpData.userInfoAll.Length;
            for(int i=0;i<count;i++)
            {
                var index = i;
                if(TmpData.userInfoAll[index].user_team==0)
                {
                    Score_Blue += TmpData.userInfoAll[index].user_point;
                }
                else
                {
                    Score_red += TmpData.userInfoAll[index].user_point;
                }   
            }
            view.m_lbl_BlueScores_LanguageText.text = Score_Blue.ToString();
            view.m_lbl_RedScores_LanguageText.text = Score_red.ToString();
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
            
        }

        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}