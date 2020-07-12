// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月15日
// Update Time         :    2020年5月15日
// Class Description   :    sendmessageMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using ProtoBuf;
using System.IO;

namespace Game {
    public class sendmessageMediator : GameMediator {
        #region Member
        public static string NameMediator = "sendmessageMediator";


        #endregion

        //IMediatorPlug needs
        
        public void SendEnterRoomPackage()
        {
            Debug.Log("发送Msg_ENTERROOM_C2S包");
            Msg.Msg_EnterRoom_C2S xMsg = new Msg.Msg_EnterRoom_C2S();
            xMsg.id = 1;


            MemoryStream stream = new MemoryStream();
            Serializer.Serialize<Msg.Msg_EnterRoom_C2S>(stream, xMsg);

            AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.ENTER_ROOM_C2S, stream);
        }

        public void SendChangeStatePackage()
        {
            Debug.Log("发送changestate包");
            Msg.Msg_ChangeState_C2S xMsg = new Msg.Msg_ChangeState_C2S();
            xMsg.type = 2;
            xMsg.number1 = 1;
            xMsg.number2 = 2;
            xMsg.ready = 0;


            MemoryStream stream = new MemoryStream();
            Serializer.Serialize<Msg.Msg_ChangeState_C2S>(stream, xMsg);

            AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.CHANGE_STATE_C2S, stream);
        }
        public void SendExpelPackage()
        {
            Debug.Log("发送expel包");
            Msg.Msg_Expel_C2S xMsg = new Msg.Msg_Expel_C2S();
            xMsg.number = 1;
            xMsg.id = 1;


            MemoryStream stream = new MemoryStream();
            Serializer.Serialize<Msg.Msg_Expel_C2S>(stream, xMsg);

            AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.EXPEL_C2S, stream);
        }

        public sendmessageMediator(object viewComponent ):base(NameMediator, viewComponent )
        {
        }


        public sendmessageView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                ((int)Msg.MsgType.ENTER_ROOM_S2C).ToString(),
                ((int)Msg.MsgType.CHANGE_STATE_S2C).ToString(),
                ((int)Msg.MsgType.EXPEL_S2C).ToString()
                 
            }.ToArray();
        }

        public override void HandleNotification(INotification notificationName)
        {
            switch ((Msg.MsgType)int.Parse(notificationName.Name))
            {
              
                case Msg.MsgType.ENTER_ROOM_S2C:
                    {
                        Msg.Msg_EnterRoom_S2C xMsg = new Msg.Msg_EnterRoom_S2C();
                        xMsg = Serializer.Deserialize<Msg.Msg_EnterRoom_S2C>((MemoryStream)notificationName.Body);
                        Debug.Log(xMsg.result);//加入房间 0成功 -1失败
                        Debug.Log(xMsg.number);//房间内玩家人数
                        Debug.Log(xMsg.szName);//用户名
                    }
                    break;

                case Msg.MsgType.CHANGE_STATE_S2C:
                    {
                        Msg.Msg_ChangeState_S2C xMsg = new Msg.Msg_ChangeState_S2C();
                        xMsg = Serializer.Deserialize<Msg.Msg_ChangeState_S2C>((MemoryStream)notificationName.Body);
                        Debug.Log(xMsg.number1);//玩家之前位置
                        Debug.Log(xMsg.number2);//玩家现在位置
                        Debug.Log(xMsg.ready);//写入玩家准备状态
                        Debug.Log(xMsg.result);//改变状态  0成功 -1失败
                        Debug.Log(xMsg.type);//玩家选择的坦克

                    }

                    break;

                case Msg.MsgType.EXPEL_S2C:
                    {
                        Msg.Msg_Expel_S2C xMsg = new Msg.Msg_Expel_S2C();
                        xMsg = Serializer.Deserialize<Msg.Msg_Expel_S2C>((MemoryStream)notificationName.Body);
                        Debug.Log(xMsg.number);//玩家ID
                        Debug.Log(xMsg.result);//踢出成功0，失败-1

                    }
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
            view.m_send_enterroom_button.AddClickEvent(SendEnterRoomPackage);
            view.m_send_changestate_button.AddClickEvent(SendChangeStatePackage);
            view.m_send_expel_button.AddClickEvent(SendExpelPackage);
        }

        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}