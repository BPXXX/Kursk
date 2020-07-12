// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月17日
// Update Time         :    2020年5月17日
// Class Description   :    UI_RegisterPanelMediator
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
using System;
using ProtoBuf;
using System.IO;

namespace Game {
    public class UI_RegisterPanelMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_RegisterPanelMediator";


        #endregion

        //IMediatorPlug needs
        public UI_RegisterPanelMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_RegisterPanelView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                ((int)Msg.MsgType.REGISTER_S2C).ToString()
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            if (notification.Type == "msg")
            {
                switch ((Msg.MsgType)int.Parse(notification.Name))
                {
                    case Msg.MsgType.REGISTER_S2C:
                        {
                            Msg.Msg_Register_S2C xMsg = new Msg.Msg_Register_S2C();
                            xMsg = Serializer.Deserialize<Msg.Msg_Register_S2C>((MemoryStream)notification.Body);
                            if (xMsg.result == 0)
                            {
                                Tip.CreateTip("注册账号成功").Show();
                                Debug.Log("1111");
                            }
                            else if (xMsg.result == -1)
                            {
                                Tip.CreateTip("注册账号失败").Show();
                                Debug.Log("1111");
                            }
                        }
                        break;

                }
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
            view.m_ipt_PasswordsInputField_GameInput.contentType = InputField.ContentType.Password;
            view.m_UI_Button_Sure.AddClickEvent(OnSureClick);
            view.m_UI_CancelButton.AddClickEvent(OnCancleClick);
        }

        private void OnCancleClick()
        {
            SendNotification(CmdConstant.MainInterFacePanel);
        }

        private void OnSureClick()
        {
            if (Check())
                SendNotification(CmdConstant.RegisterAccountCmd);
        }
        public bool Check()
        {
            if (view.m_ipt_AccountInputField_GameInput.text.Equals("") || view.m_ipt_PasswordsInputField_GameInput.text.Equals(""))
            {
                Debug.Log("222");
                Tip.CreateTip("用户名和密码不能为空").Show();
                return false;
            }
            else
            {
                TmpData.Data.Account = view.m_ipt_AccountInputField_GameInput.text;
                TmpData.Data.Passwords = view.m_ipt_PasswordsInputField_GameInput.text;
                return true;
            }

        }
        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}