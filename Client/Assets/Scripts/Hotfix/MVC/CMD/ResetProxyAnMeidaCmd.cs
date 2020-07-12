
using PureMVC.Interfaces;
using Skyunion;
using UnityEngine;
namespace Game
{
    public class ResetProxyAnMeida : GameCmd
    {
        public override void Execute(INotification notification)
        {
            var af = AppFacade.GetInstance();
            // 在这里注册所有的 Porxy
            af.RemoveProxy(NetProxy.NAME);
            af.RegisterProxy(new NetProxy(NetProxy.ProxyNAME));


            //在这里注册所有的全局mediator.
            GlobalBehaviourManger.Instance.RemoveGlobalMediator(GameToolsGlobalMediator.NameMediator);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<GameToolsGlobalMediator>(true);

            GlobalBehaviourManger.Instance.RemoveGlobalMediator(MapMgrMediator.NameMediator);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<MapMgrMediator>(true);
        }
    }
}

