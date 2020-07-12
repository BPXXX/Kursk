// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月28日
// Update Time         :    2020年5月28日
// Class Description   :    UI_GamePrepareInterfaceMediator
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
using ProtoBuf;
using System.IO;
using System;

namespace Game {
    public class UI_GamePrepareInterfaceMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_GamePrepareInterfaceMediator";

        public static int type_Tank = 1;
        public static int number_Position = 0;
        public static string name_User = "";
        bool[] IsHavingUser = new bool[12] { false, false, false, false, false, false, false, false, false, false, false, false, };
        int IsReadyState = 0;//0表示为准备，1表示准备；
        #endregion

        //IMediatorPlug needs
        public UI_GamePrepareInterfaceMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_GamePrepareInterfaceView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
               // ((int)Msg.MsgType.ENTER_ROOM_S2C).ToString(),
                ((int)Msg.MsgType.GET_ROOMMEN_S2C).ToString(),
                ((int)Msg.MsgType.CHANGE_STATE_S2C).ToString(),
                 ((int)Msg.MsgType.START_GAME_S2C).ToString(),
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            if (notification.Type == "msg")
            {
                switch ((Msg.MsgType)int.Parse(notification.Name))
                {
                   /* case Msg.MsgType.ENTER_ROOM_S2C:
                        {

                            int xmg = UI_JoinRoomPanelMediator.JoinRoomId;
                            SendNotification(CmdConstant.ShowUserInfoCmd, xmg);

                        }
                        break;*/
                    case Msg.MsgType.GET_ROOMMEN_S2C:
                        {
                            Msg.Msg_GetRoomMen_S2C xMsg = new Msg.Msg_GetRoomMen_S2C();
                            var men = (MemoryStream)notification.Body;
                            men.Position = 0;
                            xMsg = Serializer.Deserialize<Msg.Msg_GetRoomMen_S2C>(men);
                            if (xMsg.result == 0)
                            {
                                List<int> number = xMsg.number;
                                List<byte[]> names = xMsg.szName;
                                List<int> types = xMsg.tankType;
                                List<int> readyStates = xMsg.ready;
                                int size = number.Count;
                                RemoveUserInfo();
                                ShowUserInfo(size, number, names, types, readyStates);
                                Tip.CreateTip("显示用户成功").Show();
                            }
                            else if (xMsg.result == -1)
                            {

                                Tip.CreateTip("显示用户失败").Show();
                            }
                        }
                        break;
                    case Msg.MsgType.CHANGE_STATE_S2C:
                        {
                            Msg.Msg_ChangeState_S2C xMsg = new Msg.Msg_ChangeState_S2C();
                            var men = (MemoryStream)notification.Body;
                            men.Position = 0;
                            xMsg = Serializer.Deserialize<Msg.Msg_ChangeState_S2C>(men);
                            if (xMsg.result == 0)
                            {
                                if (number_Position == xMsg.number1)
                                {
                                    type_Tank = xMsg.type;
                                    number_Position = xMsg.number2;
                                }
                                int xmg = UI_JoinRoomPanelMediator.JoinRoomId;
                                SendNotification(CmdConstant.ShowUserInfoCmd, xmg);
                            }
                            else if (xMsg.result == -1)
                            {

                                Tip.CreateTip("更换位置失败").Show();
                            }
                        }
                        break;
                    case Msg.MsgType.START_GAME_S2C:
                        {
                            Msg.Msg_StartGame_S2C xMsg = new Msg.Msg_StartGame_S2C();
                            var men = (MemoryStream)notification.Body;
                            men.Position = 0;
                            xMsg = Serializer.Deserialize<Msg.Msg_StartGame_S2C>(men);
                            if (xMsg.result == 0)
                            {
                                TmpData.userInfoSelf.position_number = number_Position;
                                TmpData.userInfoSelf.tank_type = type_Tank;
                                TmpData.userInfoSelf.user_name = name_User;
                                Msg.Msg_ChangeState_C2S Xmsg1 = new Msg.Msg_ChangeState_C2S();
                                Xmsg1.number1 = number_Position;
                                Xmsg1.number2 = number_Position;
                                Xmsg1.type = type_Tank;
                                Xmsg1.ready = 0;
                                IsReadyState = 0;
                             //   ClientUtils.LoadSprite(view.m_UI_StatWarBtn.m_img_img_PolygonImage, "unready[准备按钮_0]");
                                SendNotification(CmdConstant.ChangeSeatCmd, Xmsg1);//进入游戏前先取消准备状态，之后游戏结束之后返回房间需要再次准备。
                                if (number_Position >= 1 && number_Position <= 6)
                                    TmpData.userInfoSelf.user_team = 0;
                                else
                                    TmpData.userInfoSelf.user_team = 1;
                                int id = 1;
                                SendNotification(CmdConstant.EnterScenceCmd, id);
                          }
                            else if (xMsg.result == -1)
                            {
                                
                                Tip.CreateTip("无法达到开始游戏的要求").Show();
                            }
                        }
                        break;
                }
            }
        }

        private void RemoveUserInfo()
        {
            for (int i = 0; i < 12; i++)
            {
                IsHavingUser[i] = false;
            }
            view.m_UI_Position_Blue1.m_lbl_usernameText_LanguageText.text = "";
            view.m_UI_Position_Blue1.m_lbl_tanknameText_LanguageText.text = "";
            view.m_UI_Position_Blue2.m_lbl_usernameText_LanguageText.text = "";
            view.m_UI_Position_Blue2.m_lbl_tanknameText_LanguageText.text = "";
            view.m_UI_Position_Blue3.m_lbl_usernameText_LanguageText.text = "";
            view.m_UI_Position_Blue3.m_lbl_tanknameText_LanguageText.text = "";
            view.m_UI_Position_Blue4.m_lbl_usernameText_LanguageText.text = "";
            view.m_UI_Position_Blue4.m_lbl_tanknameText_LanguageText.text = "";
            view.m_UI_Position_Blue5.m_lbl_usernameText_LanguageText.text = "";
            view.m_UI_Position_Blue5.m_lbl_tanknameText_LanguageText.text = "";
            view.m_UI_Position_Blue6.m_lbl_usernameText_LanguageText.text = "";
            view.m_UI_Position_Blue6.m_lbl_tanknameText_LanguageText.text = "";
            view.m_UI_Position_Red1.m_lbl_usenameText_LanguageText.text = "";
            view.m_UI_Position_Red1.m_lbl_tanknameText_LanguageText.text = "";

            view.m_UI_Position_Red2.m_lbl_usenameText_LanguageText.text = "";
            view.m_UI_Position_Red2.m_lbl_tanknameText_LanguageText.text = "";
            view.m_UI_Position_Red3.m_lbl_usenameText_LanguageText.text = "";
            view.m_UI_Position_Red3.m_lbl_tanknameText_LanguageText.text = "";
            view.m_UI_Position_Red4.m_lbl_usenameText_LanguageText.text = "";
            view.m_UI_Position_Red4.m_lbl_tanknameText_LanguageText.text = "";
            view.m_UI_Position_Red5.m_lbl_usenameText_LanguageText.text = "";
            view.m_UI_Position_Red5.m_lbl_tanknameText_LanguageText.text = "";
            view.m_UI_Position_Red6.m_lbl_usenameText_LanguageText.text = "";
            view.m_UI_Position_Red6.m_lbl_tanknameText_LanguageText.text = "";
            ClientUtils.LoadSprite(view.m_UI_Position_Blue1.m_img_Head_PolygonImage, "Head_Blue[head1]");
            ClientUtils.LoadSprite(view.m_UI_Position_Blue2.m_img_Head_PolygonImage, "Head_Blue[head1]");
            ClientUtils.LoadSprite(view.m_UI_Position_Blue3.m_img_Head_PolygonImage, "Head_Blue[head1]");
            ClientUtils.LoadSprite(view.m_UI_Position_Blue4.m_img_Head_PolygonImage, "Head_Blue[head1]");
            ClientUtils.LoadSprite(view.m_UI_Position_Blue5.m_img_Head_PolygonImage, "Head_Blue[head1]");
            ClientUtils.LoadSprite(view.m_UI_Position_Blue6.m_img_Head_PolygonImage, "Head_Blue[head1]");
            ClientUtils.LoadSprite(view.m_UI_Position_Red1.m_img_Head_PolygonImage, "Head_Red[head2]");
            ClientUtils.LoadSprite(view.m_UI_Position_Red2.m_img_Head_PolygonImage, "Head_Red[head2]");
            ClientUtils.LoadSprite(view.m_UI_Position_Red3.m_img_Head_PolygonImage, "Head_Red[head2]");
            ClientUtils.LoadSprite(view.m_UI_Position_Red4.m_img_Head_PolygonImage, "Head_Red[head2]");
            ClientUtils.LoadSprite(view.m_UI_Position_Red5.m_img_Head_PolygonImage, "Head_Red[head2]");
            ClientUtils.LoadSprite(view.m_UI_Position_Red6.m_img_Head_PolygonImage, "Head_Red[head2]");
        }

        private void ShowUserInfo(int count, List<int> number, List<byte[]> names, List<int> types, List<int> readyStates)
        {
            for (int i = 0; i < count; i++)
            {
                ShowSingleInfo(number[i], names[i], types[i], readyStates[i]);
            }
        }

        private void ShowSingleInfo(int v, byte[] name, int t, int readystate)
        {
            string type = "";
            string username = System.Text.Encoding.Default.GetString(name);
            if (t == 1)
            {
                type = "III号坦克M型";
            }
            else if (t == 2)
            {
                type = "T70";
            }
            if (v == 1)
            {
                view.m_UI_Position_Blue1.m_lbl_usernameText_LanguageText.text = username;
                view.m_UI_Position_Blue1.m_lbl_tanknameText_LanguageText.text = type;
                IsHavingUser[0] = true;
                if (readystate == 1)
                    ClientUtils.LoadSprite(view.m_UI_Position_Blue1.m_img_Head_PolygonImage, "Ready_blue[Ready_blue]");
            }
            else if (v == 2)
            {
                view.m_UI_Position_Blue2.m_lbl_usernameText_LanguageText.text = username;
                view.m_UI_Position_Blue2.m_lbl_tanknameText_LanguageText.text = type;
                IsHavingUser[1] = true;
                if (readystate == 1)
                    ClientUtils.LoadSprite(view.m_UI_Position_Blue2.m_img_Head_PolygonImage, "Ready_blue[Ready_blue]");
            }
            else if (v == 3)
            {
                view.m_UI_Position_Blue3.m_lbl_usernameText_LanguageText.text = username;
                view.m_UI_Position_Blue3.m_lbl_tanknameText_LanguageText.text = type;
                IsHavingUser[2] = true;
                if (readystate == 1)
                    ClientUtils.LoadSprite(view.m_UI_Position_Blue3.m_img_Head_PolygonImage, "Ready_blue[Ready_blue]");
            }
            else if (v == 4)
            {
                view.m_UI_Position_Blue4.m_lbl_usernameText_LanguageText.text = username;
                view.m_UI_Position_Blue4.m_lbl_tanknameText_LanguageText.text = type;
                IsHavingUser[3] = true;
                if (readystate == 1)
                    ClientUtils.LoadSprite(view.m_UI_Position_Blue4.m_img_Head_PolygonImage, "Ready_blue[Ready_blue]");
            }
            else if (v == 5)
            {
                view.m_UI_Position_Blue5.m_lbl_usernameText_LanguageText.text = username;
                view.m_UI_Position_Blue5.m_lbl_tanknameText_LanguageText.text = type;
                IsHavingUser[4] = true;
                if (readystate == 1)
                    ClientUtils.LoadSprite(view.m_UI_Position_Blue5.m_img_Head_PolygonImage, "Ready_blue[Ready_blue]");
            }
            else if (v == 6)
            {
                view.m_UI_Position_Blue6.m_lbl_usernameText_LanguageText.text = username;
                view.m_UI_Position_Blue6.m_lbl_tanknameText_LanguageText.text = type;
                IsHavingUser[5] = true;
                if (readystate == 1)
                    ClientUtils.LoadSprite(view.m_UI_Position_Blue6.m_img_Head_PolygonImage, "Ready_blue[Ready_blue]");
            }
            else if (v == 7)
            {
                view.m_UI_Position_Red1.m_lbl_usenameText_LanguageText.text = username;
                view.m_UI_Position_Red1.m_lbl_tanknameText_LanguageText.text = type;
                IsHavingUser[6] = true;
                if (readystate == 1)
                    ClientUtils.LoadSprite(view.m_UI_Position_Red1.m_img_Head_PolygonImage, "Ready_red[Ready_red]");
            }
            else if (v == 8)
            {
                view.m_UI_Position_Red2.m_lbl_usenameText_LanguageText.text = username;
                view.m_UI_Position_Red2.m_lbl_tanknameText_LanguageText.text = type;
                IsHavingUser[7] = true;
                if (readystate == 1)
                    ClientUtils.LoadSprite(view.m_UI_Position_Red2.m_img_Head_PolygonImage, "Ready_red[Ready_red]");
            }
            else if (v == 9)
            {
                view.m_UI_Position_Red3.m_lbl_usenameText_LanguageText.text = username;
                view.m_UI_Position_Red3.m_lbl_tanknameText_LanguageText.text = type;
                IsHavingUser[8] = true;
                if (readystate == 1)
                    ClientUtils.LoadSprite(view.m_UI_Position_Red3.m_img_Head_PolygonImage, "Ready_red[Ready_red]");
            }
            else if (v == 10)
            {
                view.m_UI_Position_Red4.m_lbl_usenameText_LanguageText.text = username;
                view.m_UI_Position_Red4.m_lbl_tanknameText_LanguageText.text = type;
                IsHavingUser[9] = true;
                if (readystate == 1)
                    ClientUtils.LoadSprite(view.m_UI_Position_Red4.m_img_Head_PolygonImage, "Ready_red[Ready_red]");

            }
            else if (v == 11)
            {
                view.m_UI_Position_Red5.m_lbl_usenameText_LanguageText.text = username;
                view.m_UI_Position_Red5.m_lbl_tanknameText_LanguageText.text = type;
                IsHavingUser[10] = true;
                if (readystate == 1)
                    ClientUtils.LoadSprite(view.m_UI_Position_Red5.m_img_Head_PolygonImage, "Ready_red[Ready_red]");
            }
            else if (v == 12)
            {
                view.m_UI_Position_Red6.m_lbl_usenameText_LanguageText.text = username;
                view.m_UI_Position_Red6.m_lbl_tanknameText_LanguageText.text = type;
                IsHavingUser[11] = true;
                if (readystate == 1)
                    ClientUtils.LoadSprite(view.m_UI_Position_Red6.m_img_Head_PolygonImage, "Ready_red[Ready_red]");
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
            view.m_UI_ChangePosition1.AddClickEvent(ChangeToOne);
            view.m_UI_ChangePosition2.AddClickEvent(ChangeToTwo);
            view.m_UI_ChangePosition3.AddClickEvent(ChangeToThree);
            view.m_UI_ChangePosition4.AddClickEvent(ChangeToFour);
            view.m_UI_ChangePosition5.AddClickEvent(ChangeToFive);
            view.m_UI_ChangePosition6.AddClickEvent(ChangeToSix);
            view.m_UI_ChangePosition7.AddClickEvent(ChangeToSeven);
            view.m_UI_ChangePosition8.AddClickEvent(ChangeToEight);
            view.m_UI_ChangePosition9.AddClickEvent(ChangeToNine);
            view.m_UI_ChangePosition10.AddClickEvent(ChangeToTen);
            view.m_UI_ChangePosition11.AddClickEvent(ChangeToEleven);
            view.m_UI_ChangePosition12.AddClickEvent(ChangeToTwelve);


            //监听选坦克
            view.m_UI_TankChooseBtn_One.AddClickEvent(ChooseOne);
            view.m_UI_TankChooseBtn_Two.AddClickEvent(ChooseTwo);
            //开始战斗监听
            view.m_UI_StatWarBtn.AddClickEvent(OnStartWar);
        }


        private void OnStartWar()
        {
            if(IsReadyState==0)
            {
                Tip.CreateTip("已准备").Show();
                IsReadyState = 1;
                Msg.Msg_ChangeState_C2S xMsg = new Msg.Msg_ChangeState_C2S();
                xMsg.number1 = number_Position;
                xMsg.number2 = number_Position;
                xMsg.type = type_Tank;
                xMsg.ready = 1;
                SendNotification(CmdConstant.ChangeSeatCmd, xMsg);//并非改变座位，改变状态就会这样
                ClientUtils.LoadSprite(view.m_UI_StatWarBtn.m_img_img_PolygonImage, "ready[ready_0]");
            }
            else
            {
                Tip.CreateTip("未准备").Show();
                IsReadyState = 0;
                Msg.Msg_ChangeState_C2S xMsg = new Msg.Msg_ChangeState_C2S();
                xMsg.number1 = number_Position;
                xMsg.number2 = number_Position;
                xMsg.type = type_Tank;
                xMsg.ready = 0;
                SendNotification(CmdConstant.ChangeSeatCmd, xMsg);//并非改变座位，改变状态就会这样
                ClientUtils.LoadSprite(view.m_UI_StatWarBtn.m_img_img_PolygonImage, "unready[准备按钮_0]");
            }
        }

        private void ChooseTwo()
        {
            Msg.Msg_ChangeState_C2S xMsg = new Msg.Msg_ChangeState_C2S();
            xMsg.number1 = number_Position;
            xMsg.number2 = number_Position;
            xMsg.type = 2;
            xMsg.ready = IsReadyState;
            SendNotification(CmdConstant.ChangeSeatCmd, xMsg);//并非改变座位，改变状态就会这样
        }

        private void ChooseOne()
        {
            Msg.Msg_ChangeState_C2S xMsg = new Msg.Msg_ChangeState_C2S();
            xMsg.number1 = number_Position;
            xMsg.number2 = number_Position;
            xMsg.type = 1;
            xMsg.ready = IsReadyState;
            SendNotification(CmdConstant.ChangeSeatCmd, xMsg);//并非改变座位，改变状态就会这样
        }

        private void ChangeToTwelve()
        {
            if (IsHavingUser[11] == false)
            {
                Msg.Msg_ChangeState_C2S xMsg = new Msg.Msg_ChangeState_C2S();
                xMsg.number1 = number_Position;
                xMsg.number2 = 12;
                xMsg.type = type_Tank;
                xMsg.ready = IsReadyState;
                SendNotification(CmdConstant.ChangeSeatCmd, xMsg);
            }
            else
            {
                Tip.CreateTip("该位置有人，无法更换").Show();
            }
        }

        private void ChangeToEleven()
        {
            if (IsHavingUser[10] == false)
            {
                Msg.Msg_ChangeState_C2S xMsg = new Msg.Msg_ChangeState_C2S();
                xMsg.number1 = number_Position;
                xMsg.number2 = 11;
                xMsg.type = type_Tank;
                xMsg.ready = IsReadyState;
                SendNotification(CmdConstant.ChangeSeatCmd, xMsg);
            }
            else
            {
                Tip.CreateTip("该位置有人，无法更换").Show();
            }
        }

        private void ChangeToTen()
        {
            if (IsHavingUser[9] == false)
            {
                Msg.Msg_ChangeState_C2S xMsg = new Msg.Msg_ChangeState_C2S();
                xMsg.number1 = number_Position;
                xMsg.number2 = 10;
                xMsg.type = type_Tank;
                xMsg.ready = IsReadyState;
                SendNotification(CmdConstant.ChangeSeatCmd, xMsg);
            }
            else
            {
                Tip.CreateTip("该位置有人，无法更换").Show();
            }
        }

        private void ChangeToNine()
        {
            if (IsHavingUser[8] == false)
            {
                Msg.Msg_ChangeState_C2S xMsg = new Msg.Msg_ChangeState_C2S();
                xMsg.number1 = number_Position;
                xMsg.number2 = 9;
                xMsg.type = type_Tank;
                xMsg.ready = IsReadyState;
                SendNotification(CmdConstant.ChangeSeatCmd, xMsg);
            }
            else
            {
                Tip.CreateTip("该位置有人，无法更换").Show();
            }
        }

        private void ChangeToEight()
        {
            if (IsHavingUser[7] == false)
            {
                Msg.Msg_ChangeState_C2S xMsg = new Msg.Msg_ChangeState_C2S();
                xMsg.number1 = number_Position;
                xMsg.number2 = 8;
                xMsg.type = type_Tank;
                xMsg.ready = IsReadyState;
                SendNotification(CmdConstant.ChangeSeatCmd, xMsg);
            }
            else
            {
                Tip.CreateTip("该位置有人，无法更换").Show();
            }
        }

        private void ChangeToSeven()
        {
            if (IsHavingUser[6] == false)
            {
                Msg.Msg_ChangeState_C2S xMsg = new Msg.Msg_ChangeState_C2S();
                xMsg.number1 = number_Position;
                xMsg.number2 = 7;
                xMsg.type = type_Tank;
                xMsg.ready = IsReadyState;
                SendNotification(CmdConstant.ChangeSeatCmd, xMsg);
            }
            else
            {
                Tip.CreateTip("该位置有人，无法更换").Show();
            }
        }
        private void ChangeToSix()
        {
            if (IsHavingUser[5] == false)
            {
                Msg.Msg_ChangeState_C2S xMsg = new Msg.Msg_ChangeState_C2S();
                xMsg.number1 = number_Position;
                xMsg.number2 = 6;
                xMsg.type = type_Tank;
                xMsg.ready = IsReadyState;
                SendNotification(CmdConstant.ChangeSeatCmd, xMsg);
            }
            else
            {
                Tip.CreateTip("该位置有人，无法更换").Show();
            }
        }



        private void ChangeToFive()
        {
            if (IsHavingUser[4] == false)
            {
                Msg.Msg_ChangeState_C2S xMsg = new Msg.Msg_ChangeState_C2S();
                xMsg.number1 = number_Position;
                xMsg.number2 = 5;
                xMsg.type = type_Tank;
                xMsg.ready = IsReadyState;
                SendNotification(CmdConstant.ChangeSeatCmd, xMsg);
            }
            else
            {
                Tip.CreateTip("该位置有人，无法更换").Show();
            }
        }

        private void ChangeToFour()
        {
            if (IsHavingUser[3] == false)
            {
                Msg.Msg_ChangeState_C2S xMsg = new Msg.Msg_ChangeState_C2S();
                xMsg.number1 = number_Position;
                xMsg.number2 = 4;
                xMsg.type = type_Tank;
                xMsg.ready = IsReadyState;
                SendNotification(CmdConstant.ChangeSeatCmd, xMsg);
            }
            else
            {
                Tip.CreateTip("该位置有人，无法更换").Show();
            }
        }

        private void ChangeToThree()
        {
            if (IsHavingUser[2] == false)
            {
                Msg.Msg_ChangeState_C2S xMsg = new Msg.Msg_ChangeState_C2S();
                xMsg.number1 = number_Position;
                xMsg.number2 = 3;
                xMsg.type = type_Tank;
                xMsg.ready = IsReadyState;
                SendNotification(CmdConstant.ChangeSeatCmd, xMsg);
            }
            else
            {
                Tip.CreateTip("该位置有人，无法更换").Show();
            }
        }

        private void ChangeToTwo()
        {
            if (IsHavingUser[1] == false)
            {
                Msg.Msg_ChangeState_C2S xMsg = new Msg.Msg_ChangeState_C2S();
                xMsg.number1 = number_Position;
                xMsg.number2 = 2;
                xMsg.type = type_Tank;
                xMsg.ready = IsReadyState;
                SendNotification(CmdConstant.ChangeSeatCmd, xMsg);
            }
            else
            {
                Tip.CreateTip("该位置有人，无法更换").Show();
            }
        }

        private void ChangeToOne()
        {
            if (IsHavingUser[0] == false)
            {
                Msg.Msg_ChangeState_C2S xMsg = new Msg.Msg_ChangeState_C2S();
                xMsg.number1 = number_Position;
                xMsg.number2 = 1;
                xMsg.type = type_Tank;
                xMsg.ready = IsReadyState;
                SendNotification(CmdConstant.ChangeSeatCmd, xMsg);
            }
            else
            {
                Tip.CreateTip("改位置有人，无法更换").Show();
            }
        }
        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}