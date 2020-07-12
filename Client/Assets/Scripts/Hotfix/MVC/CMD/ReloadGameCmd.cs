
using PureMVC.Interfaces;
using Skyunion;
using UnityEngine;
namespace Game
{
    public class ReloadGame : GameCmd
    {
        public override void Execute(INotification notification)
        {
            var netProxy = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
            if(netProxy!=null)
            {
                netProxy.CloseSever();
            }
            CoreUtils.uiManager.CloseAll();
            CoreUtils.logService.Info("ReloadGame", Color.green);
            SendNotification(CmdConstant.ResetProxyAnMeida);
            CoreUtils.uiManager.ShowUI(UI.s_MainInterface,()=>         
            {
                SendNotification(CmdConstant.AutoLogin);
            });

        }
    }
}

