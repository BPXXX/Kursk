// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月17日
// Update Time         :    2020年5月17日
// Class Description   :    UI_MainInterfaceMediator
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
    public class UI_MainInterfaceMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_MainInterfaceMediator";


        #endregion

        //IMediatorPlug needs
        public UI_MainInterfaceMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_MainInterfaceView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.ConnectEvent,        
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.ConnectEvent:
                    {
                        NetEvent @event = (NetEvent)notification.Body;
                        if(@event==NetEvent.ConnectComplete)
                        {
                            Tip.CreateTip("连接成功").Show();
                        }
                        else if(@event == NetEvent.ConnectFail)
                        {
                            Tip.CreateTip("连接失败").Show();
                        }
                    }
                    break;
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
            view.m_UI_StartGameButton.AddClickEvent(OnStartClick);
            view.m_UI_ExitGameButton.AddClickEvent(OnExitClick);
            view.m_UI_SettingButton.AddClickEvent(OnSettingClick);
            view.m_UI_AboutButton.AddClickEvent(OnAboutClick);
        }

        private void OnAboutClick()
        {
            SendNotification(CmdConstant.AboutPanel);
        }

        private void OnSettingClick()
        {
            SendNotification(CmdConstant.SettingPanel);
        }

        private void OnExitClick()
        {
            #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
            #else
                    Application.Quit();
            #endif
        }

        private void OnStartClick()
        {
            SendNotification(CmdConstant.LoginPanel);
        }

        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}