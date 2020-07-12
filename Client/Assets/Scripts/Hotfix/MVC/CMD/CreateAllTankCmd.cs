
using PureMVC.Interfaces;
using Skyunion;
using UnityEngine;
using ProtoBuf;
using Game;
using Skyunion;
using System.IO;

namespace Game
{
    public class CreateAllTankCmd : GameCmd
    {


        public override void Execute(INotification notification)
        {

            GameObject m_playerRoot;
            GameObject m_tankRoot;


            //多生成一辆单机测试用坦克，联网模式需注释掉
           //m_playerRoot = GameObject.Find("/GameObject");
           // CoreUtils.assetService.Instantiate($"tank", (GameObject) =>
           // {
           //     GameObject.transform.SetParent(m_playerRoot.transform);
           //     m_tankRoot = GameObject;


           //     m_tankRoot.GetComponent<controller>().id = 2;
           //     m_tankRoot.GetComponent<controller>().type = 1;
           //     m_tankRoot.GetComponent<controller>().hp = 100;
           //     m_tankRoot.GetComponent<controller>().mode = 2;
           //     m_tankRoot.transform.position = new Vector3(5, -10, 0);
           //     m_tankRoot.GetComponent<controller>().team = 0;


           // });


            Msg.MSG_USERINFOLISTOS_S2C xMsg = (Msg.MSG_USERINFOLISTOS_S2C)notification.Body;
            //Msg.Msg_USERINFO_S2C player = new Msg.Msg_USERINFO_S2C();
            int PlayerNumber = xMsg.userinfo.Count;     //list长度=玩家个数

            for(int i=0;i<12;i++)
            {
                var index = i;
                TmpData.userInfoAll[index].user_id = -1;
            }

            for (int i = 0; i < PlayerNumber; i++)
            {
                var index = i;
                TmpData.userInfoAll[index].user_id = xMsg.userinfo[index].id;
                TmpData.userInfoAll[index].tank_type = xMsg.userinfo[index].type;
                TmpData.userInfoAll[index].user_team = xMsg.userinfo[index].team;
                TmpData.userInfoAll[index].user_point = 0;



                Debug.Log("初始化了" + index + "辆坦克");
                m_playerRoot = GameObject.Find("/GameObject");

                CoreUtils.assetService.Instantiate($"tank", (GameObject) =>
                {
                    GameObject.transform.SetParent(m_playerRoot.transform);
                    m_tankRoot = GameObject;


                    m_tankRoot.GetComponent<controller>().id = xMsg.userinfo[index].id;
                    m_tankRoot.GetComponent<controller>().type = xMsg.userinfo[index].type;
                    m_tankRoot.GetComponent<controller>().hp = xMsg.userinfo[index].hp;
                    m_tankRoot.GetComponent<controller>().team = xMsg.userinfo[index].team;
                    TmpData.userInfoAll[index].user_team = xMsg.userinfo[index].team;

                    string SUsername = System.Text.Encoding.Default.GetString(xMsg.userinfo[index].username);
                    Debug.Log(SUsername);
                    Debug.Log(TmpData.userInfoSelf.user_name);
                    if (string.Equals(TmpData.userInfoSelf.user_name, SUsername) == true)
                    {
                        m_tankRoot.GetComponent<controller>().mode = 1;//本地坦克
                    }
                    else
                    {
                        m_tankRoot.GetComponent<controller>().mode = 2;//mode =2    other坦克
                    }

                });


            }
            Debug.Log("发送完成初始化坦克模型的包");
            Msg.MSG_INFOAC_C2S InfoAcMsg = new Msg.MSG_INFOAC_C2S();
            InfoAcMsg.result = 1;

            MemoryStream stream = new MemoryStream();
            Serializer.Serialize<Msg.MSG_INFOAC_C2S>(stream, InfoAcMsg);
            AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.InfoAc, stream);


        }

    }

}