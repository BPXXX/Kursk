
using PureMVC.Interfaces;
using Skyunion;
using UnityEngine;

namespace Game
{
    public class InitPositionCmd : GameCmd
    {


        public override void Execute(INotification notification)
        {
            Debug.Log("进入InitPositionCmd");
            GameObject[] m_TankDesk; //存放坦克物件
            m_TankDesk = GameObject.FindGameObjectsWithTag("Tank"); //查找所有关于tag为tank的物件，保存到m_TanakDesk
            Debug.Log(m_TankDesk[0].name);
            Msg.MSG_VECOSLIST_S2C xMsg = (Msg.MSG_VECOSLIST_S2C)notification.Body;

            int PlayerNumber = xMsg.vec.Count; //需要初始化位置的坦克数量
            for(int j=0;j<m_TankDesk.Length;j++)
            {
                for (int i = 0; i < PlayerNumber; i++)
                {
                    var index = i;
                    TmpData.userInfoAll[index].init_position = new Vector3(xMsg.vec[index].x, xMsg.vec[index].y, 0);
                    if(m_TankDesk[j].GetComponent<controller>().id ==xMsg.vec[i].id)

                        m_TankDesk[j].transform.position = new Vector3(xMsg.vec[i].x, xMsg.vec[i].y, 0);

                }
            }
           

        }





    }
}



