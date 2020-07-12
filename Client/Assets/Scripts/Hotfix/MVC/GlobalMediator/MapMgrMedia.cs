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
    public class MapMgrMedia : GameMediator
    {
        #region Member
        public static string NameMediator = "MapMgrMedia";
        private GameObject m_scenceRoot;
        private GameObject m_mapRoot;
        private GameObject m_playerRoot;
        


        #endregion

        public MapMgrMedia() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }
        //IMediatorPlug needs
        public MapMgrMedia(object viewComponent) : base(NameMediator, null) { }

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.EnterScenceCmd,
                ((int)Msg.MsgType.VEC_S2C).ToString(),
            }.ToArray();

        }

        public override void HandleNotification(INotification notification)
        {


            switch (notification.Name)
            {
                case CmdConstant.EnterScenceCmd:
                    {
                        int sceneIndex = (int)notification.Body;

                        CoreUtils.assetService.Instantiate($"scenes_{sceneIndex}", (GameObject) =>
                         {
                             GameObject.transform.SetParent(m_scenceRoot.transform);
                             m_mapRoot = GameObject;
                         }
                         );

                        CoreUtils.assetService.Instantiate($"player", (GameObject) =>
                        {
                            GameObject.transform.SetParent(m_playerRoot.transform);
                            m_playerRoot = GameObject;
                        }
                         );
                    }
                    break;
                default:

                    break;

            }

            if (notification.Type == "msg")
            {
                switch ((Msg.MsgType)int.Parse(notification.Name))
                {
                    case (Msg.MsgType.VEC_S2C):
                        {

                            Msg.Msg_VEC_S2C xMsg = new Msg.Msg_VEC_S2C();
                            xMsg = Serializer.Deserialize<Msg.Msg_VEC_S2C>((MemoryStream)notification.Body);

                           // SendNotification(CmdConstant.S2CcontrolTank, xMsg);   //收到服务器发来的移动包后进行操作坦克
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



           

        }

        protected override void BindUIEvent()
        {

        }

        protected override void BindUIData()
        {
            m_scenceRoot = GameObject.Find("/SceneObject");
            m_playerRoot = GameObject.Find("/GameObject");

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