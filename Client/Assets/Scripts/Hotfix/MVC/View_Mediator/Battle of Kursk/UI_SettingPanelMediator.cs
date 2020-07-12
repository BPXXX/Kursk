// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月29日
// Update Time         :    2020年5月29日
// Class Description   :    UI_SettingPanelMediator
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
    public class UI_SettingPanelMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_SettingPanelMediator";


        #endregion

        //IMediatorPlug needs
        public UI_SettingPanelMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_SettingPanelView view;

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
            view.m_UI_CancelButton.AddClickEvent(OnCancleClick);
        }

        private void OnCancleClick()
        {
            //这里应该加一个取消数据更改的功能
            SendNotification(CmdConstant.MainInterFacePanel);
        }

        private void OnSureClick()
        {
            //这里应该加一个保存数据更改的功能
            SendNotification(CmdConstant.MainInterFacePanel);
        }

        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}