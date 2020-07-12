// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月17日
// Update Time         :    2020年5月17日
// Class Description   :    UI_ LoginPanelMediator
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
using UnityEngine.UI;
using ProtoBuf;
using System.IO;

namespace Game {
    public class UI_LoginPanelMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_LoginPanelMediator";


        #endregion

        //IMediatorPlug needs
        public UI_LoginPanelMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_LoginPanelView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                ((int)Msg.MsgType.LOGIN_S2C).ToString()
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            if (notification.Type == "msg")
            {
                switch ((Msg.MsgType)int.Parse(notification.Name))
                {
                    case Msg.MsgType.LOGIN_S2C:
                        {
                            Msg.Msg_Login_S2C xMsg = new Msg.Msg_Login_S2C();
                            xMsg = Serializer.Deserialize<Msg.Msg_Login_S2C>((MemoryStream)notification.Body);
                            if(xMsg.result==0)
                            {
                                TmpData.userInfoSelf.user_id = xMsg.id;
                                Tip.CreateTip("登陆成功").Show();
                                SendNotification(CmdConstant.GameHallPanel);
                            }
                            else if(xMsg.result==-1)
                            {
                                Tip.CreateTip("账户或密码输入错误").Show();
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
            view.m_UI_RegistrationButton.AddClickEvent(OnRegisterClick);
        }

        private void OnCancleClick()
        {
            SendNotification(CmdConstant.MainInterFacePanel);
        }

        private void OnRegisterClick()
        {
            SendNotification(CmdConstant.RegisterPanel);
        }

        private void OnSureClick()
        {
            if (Check())
                SendNotification(CmdConstant.QueryAccountCmd);
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
                UI_GamePrepareInterfaceMediator.name_User = TmpData.Data.Account;
                UI_GamePrepareInterface_adminMediator.name_User = TmpData.Data.Account;
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