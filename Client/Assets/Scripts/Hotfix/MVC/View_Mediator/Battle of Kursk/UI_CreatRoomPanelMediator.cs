// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月23日
// Update Time         :    2020年5月23日
// Class Description   :    UI_CreatRoomPanelMediator
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
using ProtoBuf;
using System.IO;

namespace Game {
    public class UI_CreatRoomPanelMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_CreatRoomPanelMediator";


        #endregion
        public static int CreateRoomid = 0;

        //IMediatorPlug needs
        public UI_CreatRoomPanelMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_CreatRoomPanelView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                ((int)Msg.MsgType.CREATE_ROOM_S2C).ToString(),
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            if (notification.Type == "msg")
            {
                switch ((Msg.MsgType)int.Parse(notification.Name))
                {
                    case Msg.MsgType.CREATE_ROOM_S2C:
                        {
                            Msg.Msg_CreateRoom_S2C xMsg = new Msg.Msg_CreateRoom_S2C();
                            xMsg = Serializer.Deserialize<Msg.Msg_CreateRoom_S2C>((MemoryStream)notification.Body);
                            if (xMsg.result == 0)
                            {
                                UI_GamePrepareInterface_adminMediator.number_Position = 1;
                                Tip.CreateTip("创建房间成功").Show();
                                SendNotification(CmdConstant.GamePrepareAdminPanle);
                            }
                            else if (xMsg.result == -1)
                            {
                                CreateRoomid = 0;
                                Tip.CreateTip("创建房间失败").Show();
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
            view.m_UI_Button_Sure.AddClickEvent(OnSureClick);
            view.m_UI_CancelButton.AddClickEvent(OnCancleClick);
        }

        private void OnCancleClick()
        {
            SendNotification(CmdConstant.GameHallPanel);
        }

        private void OnSureClick()
        {
            if (view.m_ipt_RoomName_GameInput.text.Equals(""))
            {
                Tip.CreateTip("房间名不能为空").Show();
                return;
            }
            else
            {
                Debug.Log("房间名称合法");
                int xmg = int.Parse(view.m_ipt_RoomName_GameInput.text);
                CreateRoomid = xmg;
                SendNotification(CmdConstant.CreatRoomCmd,xmg);
            }
        }

        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}