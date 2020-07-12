// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月22日
// Update Time         :    2020年5月22日
// Class Description   :    UI_AboutPanelMediator
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
    public class UI_AboutPanelMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_AboutPanelMediator";


        #endregion

        //IMediatorPlug needs
        public UI_AboutPanelMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_AboutPanelView view;

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
            view.m_UI_Button_Sure.AddClickEvent(OnSureClick);
        }

        private void OnSureClick()
        {
            SendNotification(CmdConstant.MainInterFacePanel);
        }

        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}