
using PureMVC.Interfaces;
using Skyunion;
using UnityEngine;

namespace Game
{
    public class CreateAmmoCmd : GameCmd
    {


        public override void Execute(INotification notification)
        {
            GameObject m_playerRoot;
            GameObject m_AmmoRoot;
            Msg.Msg_AMMO_S2C xMsg = (Msg.Msg_AMMO_S2C)notification.Body;

            if(xMsg.type==1)
            {
                switch(xMsg.direction)
                {
                    case 1:
                {
                    m_playerRoot = GameObject.Find("/GameObject");
                    CoreUtils.assetService.Instantiate($"chuanjia_u", (GameObject) =>
                    {
                        GameObject.transform.SetParent(m_playerRoot.transform);
                        m_AmmoRoot = GameObject;
                        m_AmmoRoot.GetComponent<zidan_u>().hurt_id = xMsg.id;
                        m_AmmoRoot.transform.position = new Vector3(xMsg.x, xMsg.y, 0);

                    });
                }
                        break;
                    case 2:
                        {
                            m_playerRoot = GameObject.Find("/GameObject");
                            CoreUtils.assetService.Instantiate($"chuanjia_d", (GameObject) =>
                            {
                                GameObject.transform.SetParent(m_playerRoot.transform);
                                m_AmmoRoot = GameObject;
                                m_AmmoRoot.GetComponent<zidan_d>().hurt_id = xMsg.id;
                                m_AmmoRoot.transform.position = new Vector3(xMsg.x, xMsg.y, 0);
                            });
                        }
                        break;
                    case 3:
                        {
                            m_playerRoot = GameObject.Find("/GameObject");
                            CoreUtils.assetService.Instantiate($"chuanjia_l", (GameObject) =>
                            {
                                GameObject.transform.SetParent(m_playerRoot.transform);
                                m_AmmoRoot = GameObject;
                                m_AmmoRoot.GetComponent<zidan_l>().hurt_id = xMsg.id;
                                m_AmmoRoot.transform.position = new Vector3(xMsg.x, xMsg.y, 0);
                            });
                        }
                        break;
                    case 4:
                        {
                            m_playerRoot = GameObject.Find("/GameObject");
                            CoreUtils.assetService.Instantiate($"chuanjia_r", (GameObject) =>
                            {
                                GameObject.transform.SetParent(m_playerRoot.transform);
                                m_AmmoRoot = GameObject;
                                m_AmmoRoot.GetComponent<zidan_r>().hurt_id = xMsg.id;
                                m_AmmoRoot.transform.position = new Vector3(xMsg.x, xMsg.y, 0);
                            });
                        }
                        break;
                }
                
               
            }

           

        }





    }
}



