
using PureMVC.Interfaces;
using Skyunion;
using UnityEngine;

namespace Game
{
    public class S2CcontrolTankCmd : GameCmd
    {
        public override void Execute(INotification notification)
        {
            Debug.Log("进入S2CcontrolTankCmd");
            GameObject[] m_TankDesk; //存放坦克物件
            m_TankDesk = GameObject.FindGameObjectsWithTag("Tank"); //查找所有关于tag为tank的物件，保存到m_TanakDesk


            int dir, id;
            Msg.Msg_VEC_S2C xMsg = (Msg.Msg_VEC_S2C)notification.Body;

            id = xMsg.id;

            for (int i = 0; i < m_TankDesk.Length; i++)
            {

                if (m_TankDesk[i].GetComponent<controller>().mode == 2 && m_TankDesk[i].GetComponent<controller>().id == id)
                {// 
                    m_TankDesk[i].GetComponent<controller>().startdirection = xMsg.direction;
                    m_TankDesk[i].transform.position = new Vector3(xMsg.x, xMsg.y, 0);

                    Debug.Log(m_TankDesk[i].GetComponent<controller>().id);
                    Debug.Log(m_TankDesk[i].transform.position);
                }
            }




        }





    }
}


