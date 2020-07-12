// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, April 04, 2019
// Update Time         :    Thursday, April 04, 2019
// Class Description   :    GMGlobalMediator
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

namespace Game
{
    public class GameToolsGlobalMediator : GameMediator
    {
        #region Member
        public static string NameMediator = "GMGlobalMediator";


        #endregion

        public GameToolsGlobalMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }
        //IMediatorPlug needs
        public GameToolsGlobalMediator(object viewComponent) : base(NameMediator, null) { }

        //public override string[] ListNotificationInterests()
        //{
        //    return new List<string>(){
        //        ((int)Msg.MsgType.LOGIN_S2C).ToString()
        //    }.ToArray();
        //}

        //public override void HandleNotification(INotification notificationName)
        //{
        //    if(notificationName.Type == "msg")
        //    {
        //        switch((Msg.MsgType)int.Parse(notificationName.Name))
        //        {
        //            case Msg.MsgType.LOGIN_S2C:
        //                {
        //                    Msg.Msg_Login_S2C xMsg = new Msg.Msg_Login_S2C();
        //                    xMsg = Serializer.Deserialize<Msg.Msg_Login_S2C>((MemoryStream)notificationName.Body);
        //                }
        //                break;

        //        }
        //    }
        //}
        //public override string[] ListNotificationInterests()
        //{
        //    return new List<string>(){
        //        ((int)Msg.MsgType.ENTER_ROOM_S2C).ToString()
        //    }.ToArray();
        //}

        //public override void HandleNotification(INotification notificationName)
        //{
        //    if (notificationName.Type == "msg")
        //    {
        //        switch ((Msg.MsgType)int.Parse(notificationName.Name))
        //        {
        //            case Msg.MsgType.ENTER_ROOM_S2C:
        //                {
        //                    Msg.Msg_EnterRoom_S2C xMsg = new Msg.Msg_EnterRoom_S2C();
        //                    xMsg = Serializer.Deserialize<Msg.Msg_EnterRoom_S2C>((MemoryStream)notificationName.Body);
        //                    Debug.Log(xMsg.result);//加入房间 0成功 -1失败
        //                    Debug.Log(xMsg.number);//房间内玩家人数
        //                    Debug.Log(xMsg.szName);//用户名
        //                }

        //                break;

        //        }
        //    }
        //}
        //public override string[] ListNotificationInterests()
        //{
        //    return new List<string>(){
        //        ((int)Msg.MsgType.CHANGE_STATE_S2C).ToString()
        //    }.ToArray();
        //}

        //public override void HandleNotification(INotification notificationName)
        //{
        //    if (notificationName.Type == "msg")
        //    {
        //        switch ((Msg.MsgType)int.Parse(notificationName.Name))
        //        {
        //            case Msg.MsgType.CHANGE_STATE_S2C:
        //                {
        //                    Msg.Msg_ChangeState_S2C xMsg = new Msg.Msg_ChangeState_S2C();
        //                    xMsg = Serializer.Deserialize<Msg.Msg_ChangeState_S2C>((MemoryStream)notificationName.Body);
        //                    Debug.Log(xMsg.number1);//玩家之前位置
        //                    Debug.Log(xMsg.number2);//玩家现在位置
        //                    Debug.Log(xMsg.ready);//写入玩家准备状态
        //                    Debug.Log(xMsg.result);//改变状态  0成功 -1失败
        //                    Debug.Log(xMsg.type);//玩家选择的坦克

        //                }

        //                break;

        //        }
        //    }
        //}
        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                //((int)Msg.MsgType.EXPEL_S2C).ToString()
                //((int)Msg.MsgType.CHANGE_STATE_S2C).ToString()
            }.ToArray();

        }

        public override void HandleNotification(INotification notificationName)
        {
            if (notificationName.Type == "msg")
            {
                switch ((Msg.MsgType)int.Parse(notificationName.Name))
                {
                    default:
                    //case Msg.MsgType.EXPEL_S2C:
                    //    {
                    //        Msg.Msg_Expel_S2C xMsg = new Msg.Msg_Expel_S2C();
                    //        xMsg = Serializer.Deserialize<Msg.Msg_Expel_S2C>((MemoryStream)notificationName.Body);
                    //        Debug.Log(xMsg.number);//玩家ID
                    //        Debug.Log(xMsg.result);//踢出成功0，失败-1

                    //    }
        //                 case Msg.MsgType.CHANGE_STATE_S2C:
        ////                {
        ////                    Msg.Msg_ChangeState_S2C xMsg = new Msg.Msg_ChangeState_S2C();
        ////                    xMsg = Serializer.Deserialize<Msg.Msg_ChangeState_S2C>((MemoryStream)notificationName.Body);
        ////                    Debug.Log(xMsg.number1);//玩家之前位置
        ////                    Debug.Log(xMsg.number2);//玩家现在位置
        ////                    Debug.Log(xMsg.ready);//写入玩家准备状态
        ////                    Debug.Log(xMsg.result);//改变状态  0成功 -1失败
        ////                    Debug.Log(xMsg.type);//玩家选择的坦克

        ////                }

                        break;

                }
            }
        }

        #region UI template method          

        protected override void InitData()
        {
            IsOpenUpdate = true;
        }

        protected override void BindUIEvent()
        {

        }

        protected override void BindUIData()
        {
            
        }

        public override void Update()
        {
            if (Input.GetKeyUp(KeyCode.F5))
            {
                CoreUtils.uiManager.ShowUI(UI.s_gameTool);
            }
            if (Input.GetKeyUp(KeyCode.F11))
            {
                SendNotification(CmdConstant.ShowUserInfoCmd, 123);

            }
            //if(Input.GetKeyUp(KeyCode.F7))
            //{
            //    Tip.CreateTip("显示tip").Show();
            //}
            //if (Input.GetKeyUp(KeyCode.F8))
            //{
            //    Alert.CreateAlert("测试","测试").SetLeftButton().SetRightButton(()=> { Tip.CreateTip("显示tip").Show(); }).Show();
            //}
            //if(Input.GetKeyUp(KeyCode.F10))
            //{
            //    HelpTip.CreateTip("currencyProxy.LackOfResources(100000,1000,10000,10000000); ", Input.mousePosition).SetStyle(HelpTipData.Style.arrowDown).SetOffset(20f).SetWidth(18f).Show();
            //}
            //if (Input.GetKeyUp(KeyCode.F10))
            //{
            //    Debug.Log("发送Msg_Login_C2S包");
            //    Msg.Msg_Login_C2S xMsg = new Msg.Msg_Login_C2S();
            //    xMsg.szName = "test";
            //    xMsg.szPassword = "mima";


            //    MemoryStream stream = new MemoryStream();
            //    Serializer.Serialize<Msg.Msg_Login_C2S>(stream, xMsg);

            //    AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.LOGIN_C2S, stream);

            //}
            //if (Input.GetKeyUp(KeyCode.F10))
            //{
            //    Debug.Log("发送Msg_ENTERROOM_C2S包");
            //    Msg.Msg_EnterRoom_C2S xMsg = new Msg.Msg_EnterRoom_C2S();
            //    xMsg.id = 1;


            //    MemoryStream stream = new MemoryStream();
            //    Serializer.Serialize<Msg.Msg_EnterRoom_C2S>(stream, xMsg);

            //    AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.ENTER_ROOM_C2S, stream);

            //}
            //if (Input.GetKeyUp(KeyCode.F10))
            //{
            //    Debug.Log("发送changestate包");
            //    Msg.Msg_ChangeState_C2S xMsg = new Msg.Msg_ChangeState_C2S();
            //    xMsg.type = 2;
            //    xMsg.number1 = 1;
            //    xMsg.number2 = 2;
            //    xMsg.ready = true;


            //    MemoryStream stream = new MemoryStream();
            //    Serializer.Serialize<Msg.Msg_ChangeState_C2S>(stream, xMsg);

            //    AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.CHANGE_STATE_C2S, stream);

            //}

            if (Input.GetKeyUp(KeyCode.F10))
            {
                Debug.Log("发送expel包");
                Msg.Msg_Expel_C2S xMsg = new Msg.Msg_Expel_C2S();
                xMsg.number = 1;
                xMsg.id = 1;


                MemoryStream stream = new MemoryStream();
                Serializer.Serialize<Msg.Msg_Expel_C2S>(stream, xMsg);

                AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.EXPEL_C2S, stream);

            }
        }

        public override void LateUpdate()
        {

        }

        public override void FixedUpdate()
        {

        }



        #endregion
    }
}