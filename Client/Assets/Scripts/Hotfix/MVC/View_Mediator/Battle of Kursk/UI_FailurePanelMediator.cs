// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月3日
// Update Time         :    2020年6月3日
// Class Description   :    UI_FailurePanelMediator
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
    public class UI_FailurePanelMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_FailurePanelMediator";


        #endregion

        //IMediatorPlug needs
        public UI_FailurePanelMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_FailurePanelView view;

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
            view.m_UI_ExitGameButton.AddClickEvent(OnExitClick);
        }


        private void OnExitClick()
        {
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
            #else
                        Application.Quit();
            #endif
        }


        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}