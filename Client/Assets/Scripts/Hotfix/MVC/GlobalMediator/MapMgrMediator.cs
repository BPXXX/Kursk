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
    public class MapMgrMediator : GameMediator
    {
        #region Member
        public static string NameMediator = "MapMgrMediator";
        private GameObject m_sceneRoot;
        private GameObject m_mapRoot;
        GameObject m_playerRoot;

        #endregion

        public MapMgrMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }
        //IMediatorPlug needs
        public MapMgrMediator(object viewComponent) : base(NameMediator, null) { }

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.EnterScenceCmd,
                ((int)Msg.MsgType.VEC_S2C).ToString(),
                 ((int)Msg.MsgType.Damage_S2C).ToString(),
                 ((int)Msg.MsgType.UserInfoOS).ToString(),
                 ((int)Msg.MsgType.VecOsList).ToString(),
                 ((int)Msg.MsgType.Ammo_S2C).ToString(),
                 ((int)Msg.MsgType.PointDispute).ToString(),
                 ((int)Msg.MsgType.EndGame).ToString(),
                ((int)Msg.MsgType.AddSocres).ToString(),
                //((int)Msg.MsgType.EXPEL_S2C).ToString()
                //((int)Msg.MsgType.CHANGE_STATE_S2C).ToString()
            }.ToArray();

        }

        public override void HandleNotification(INotification notification)
        {
           switch(notification.Name)
            {
                case CmdConstant.EnterScenceCmd:
                    {
                        SendNotification(CmdConstant.GameOningPanel);
                        int sceneIndex = (int)notification.Body;
                        CoreUtils.assetService.Instantiate($"map_{sceneIndex}", (gameObejct) =>
                         {
                             gameObejct.transform.SetParent(m_sceneRoot.transform);
                             m_mapRoot = gameObejct;
                         });



                    }
                    break;

            }

            if (notification.Type == "msg")
            {
                switch ((Msg.MsgType)int.Parse(notification.Name))
                {
                    case (Msg.MsgType.VEC_S2C):
                        {
                            Debug.Log("收到服务器发来的移动包");
                            Msg.Msg_VEC_S2C xMsg = new Msg.Msg_VEC_S2C();
                            xMsg = Serializer.Deserialize<Msg.Msg_VEC_S2C>((MemoryStream)notification.Body);

                            SendNotification(CmdConstant.S2CcontrolTank, xMsg);   //收到服务器发来的移动包后进行操作坦克
                        }
                        break;
                    case (Msg.MsgType.Damage_S2C):
                        {
                            Debug.Log("收到服务器发来的伤害包");
                            Msg.Msg_DAMAGE_S2C xMsg = new Msg.Msg_DAMAGE_S2C();
                            xMsg = Serializer.Deserialize<Msg.Msg_DAMAGE_S2C>((MemoryStream)notification.Body);
                            SendNotification(CmdConstant.S2CcontrolHp, xMsg);
                        }
                        break;
                    case (Msg.MsgType.UserInfoOS):
                        {
                            Debug.Log("收到服务器发来的初始化玩家信息包");
                            Msg.MSG_USERINFOLISTOS_S2C xMsg = new Msg.MSG_USERINFOLISTOS_S2C();
                            xMsg = Serializer.Deserialize<Msg.MSG_USERINFOLISTOS_S2C>((MemoryStream)notification.Body);

                             SendNotification(CmdConstant.CreateAllTank ,xMsg);
                        }
                        break;
                    case (Msg.MsgType.VecOsList):
                        {
                            Debug.Log("收到服务器发来的初始化位置包");
                            Msg.MSG_VECOSLIST_S2C xMsg = new Msg.MSG_VECOSLIST_S2C();
                            xMsg = Serializer.Deserialize<Msg.MSG_VECOSLIST_S2C>((MemoryStream)notification.Body);

                            // SendNotification(CmdConstant.InitPosition, xMsg);
                            GameObject[] m_TankDesk; //存放坦克物件
                            m_TankDesk = GameObject.FindGameObjectsWithTag("Tank"); //查找所有关于tag为tank的物件，保存到m_TanakDesk
                            Debug.Log(m_TankDesk[0].tag);

                            int PlayerNumber = xMsg.vec.Count; //需要初始化位置的坦克数量
                            for (int j = 0; j < m_TankDesk.Length; j++)
                            {
                                for (int i = 0; i < PlayerNumber; i++)
                                {
                                    if (m_TankDesk[j].GetComponent<controller>().id == xMsg.vec[i].id)

                                        m_TankDesk[j].transform.position = new Vector3(xMsg.vec[i].x, xMsg.vec[i].y, 0);

                                }
                            }
                        }
                        break;

                    case Msg.MsgType.Ammo_S2C:
                        {
                            Msg.Msg_AMMO_S2C xMsg = new Msg.Msg_AMMO_S2C();
                            xMsg=Serializer.Deserialize<Msg.Msg_AMMO_S2C>((MemoryStream)notification.Body);

                            SendNotification(CmdConstant.CreateAmmo, xMsg);
                        }
                        break;

                    case Msg.MsgType.AddSocres:
                        {
                            Debug.Log("收到加分包");
                            Msg.Msg_ADDSCORES_S2C xMsg = new Msg.Msg_ADDSCORES_S2C();
                            xMsg = Serializer.Deserialize<Msg.Msg_ADDSCORES_S2C>((MemoryStream)notification.Body);

                            SendNotification(CmdConstant.AddScores, xMsg);
                        }
                        break;

                    case Msg.MsgType.EndGame:
                        {
                            //跳转到结算界面=》
                            Msg.Msg_ENDGAME_S2C xMsg = new Msg.Msg_ENDGAME_S2C();
                            xMsg = Serializer.Deserialize<Msg.Msg_ENDGAME_S2C>((MemoryStream)notification.Body);
                            if (xMsg.team == TmpData.userInfoSelf.user_team)
                                SendNotification(CmdConstant.SucessPanel);
                            else SendNotification(CmdConstant.FailurePanel);

                            //点击返回房间回到房间

                        }
                        break;

                }
            }
        }

        #region UI template method          
        public override void OnRemove()
        {
            base.OnRemove();
            if(m_mapRoot!=null)
            {
                CoreUtils.assetService.Destroy(m_mapRoot);
            }

        }

        protected override void InitData()
        {
            IsOpenUpdate = true;
            Debug.Log("MapMedia初始化成功！");
        }

        protected override void BindUIEvent()
        {

        }

        protected override void BindUIData()
        {
            m_sceneRoot = GameObject.Find("/SceneObject");

        }

        public override void Update()
        {
          
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