
using PureMVC.Interfaces;
using Skyunion;
using UnityEngine;

namespace Game
{
    public class CreateOtherTankCmd : GameCmd
    {
        public override void Execute(INotification notification)
        {
            GameObject m_playerRoot;
            GameObject m_tankRoot;

            m_playerRoot = GameObject.Find("/GameObject");
            CoreUtils.assetService.Instantiate($"tank", (GameObject) =>
            {
                GameObject.transform.SetParent(m_playerRoot.transform);
                m_tankRoot = GameObject;
                m_tankRoot.GetComponent<controller>().mode = 2;  //mode =1  本地坦克
                m_tankRoot.GetComponent<controller>().id = 0;
                m_tankRoot.GetComponent<controller>().type = 2;

            });

        }
    }
}

