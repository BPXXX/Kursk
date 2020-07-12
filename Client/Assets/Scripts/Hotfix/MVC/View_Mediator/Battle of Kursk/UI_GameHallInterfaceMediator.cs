// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月22日
// Update Time         :    2020年5月22日
// Class Description   :    UI_GameHallInterfaceMediator
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
    public class UI_GameHallInterfaceMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_GameHallInterfaceMediator";


        #endregion

        //IMediatorPlug needs
        public UI_GameHallInterfaceMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_GameHallInterfaceView view;

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
            view.m_UI_BackMainMenuBtn.AddClickEvent(OnBackMenuClick);
            view.m_UI_CreatRoomBtn.AddClickEvent(OnCreatClick);
            view.m_UI_SingleRoom.m_UI_JoinRoomBtn.AddClickEvent(OnJoinClick);

        }

        private void OnJoinClick()
        {
            SendNotification(CmdConstant.JoinRoomPanel);
        }

        private void OnCreatClick()
        {
            SendNotification(CmdConstant.CreateRoomPanel);
        }

        private void OnBackMenuClick()
        {
           // SendNotification(CmdConstant.ReloadGame);//应该是要采用这个reload的，但是我server重连会中断，就只能用下面这个了。
            SendNotification(CmdConstant.MainInterFacePanel);
        }

        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}