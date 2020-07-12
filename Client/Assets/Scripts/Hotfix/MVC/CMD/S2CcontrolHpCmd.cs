
using PureMVC.Interfaces;
using Skyunion;
using UnityEngine;

namespace Game
{
    public class S2CcontrolHpCmd : GameCmd
    {
        public override void Execute(INotification notification)
        {
        Debug.Log("进入S2CcontrolHpCmd");
            GameObject[] m_TankDesk; //存放坦克物件
            m_TankDesk = GameObject.FindGameObjectsWithTag("Tank"); //查找所有关于tag为tank的物件，保存到m_TanakDesk   

            Msg.Msg_DAMAGE_S2C xMsg = (Msg.Msg_DAMAGE_S2C)notification.Body;
   
            for(int i =0;i<m_TankDesk.Length;i++)
            {
                if (m_TankDesk[i].GetComponent<controller>().id == xMsg.id)
                    m_TankDesk[i].GetComponent<controller>().hp -= xMsg.damage;
            }
           
          



        }
    }
}

