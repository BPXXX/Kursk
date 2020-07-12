
using PureMVC.Interfaces;
using Skyunion;
using UnityEngine;

namespace Game
{
    public class InitTankCmd : GameCmd
    {
        public override void Execute(INotification notification)
        {
            GameObject[] m_TankDesk;
            m_TankDesk = GameObject.FindGameObjectsWithTag("Tank");
            Msg.MSG_USERINFOLISTOS_S2C xMsg = (Msg.MSG_USERINFOLISTOS_S2C)notification.Body;
            int PlayerNumber = xMsg.userinfo.Count;
            for (int i = 0; i < PlayerNumber; i++)
            {
                
                var index = i;
                Debug.Log("初始化了" + index + "辆坦克");
                Debug.Log(m_TankDesk[index].name);
                m_TankDesk[index].GetComponent<controller>().id = xMsg.userinfo[index].id;
                m_TankDesk[index].GetComponent<controller>().type = xMsg.userinfo[index].type;
                m_TankDesk[index].GetComponent<controller>().hp = xMsg.userinfo[index].hp;

                string SUsername = System.Text.Encoding.Default.GetString(xMsg.userinfo[index].username);
                if (string.Equals(TmpData.userInfoSelf.user_name, SUsername) == true)
                {
                    m_TankDesk[index].GetComponent<controller>().mode = 1;//本地坦克
                }
                else
                {
                    m_TankDesk[index].GetComponent<controller>().mode = 2;//mode =2    other坦克
                }
            }

        }
    }
}

