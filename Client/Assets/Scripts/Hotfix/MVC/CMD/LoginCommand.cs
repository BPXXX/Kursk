
using PureMVC.Interfaces;
using Skyunion;
using UnityEngine;
namespace Game
{
    public class LoginCommand : GameCmd
    {
        public override void Execute(INotification notification)
        {
           switch(notification.Name)
            {
                case CmdConstant.AutoLogin:
                    {
                        SendNotification(CmdConstant.LoginToServer);
                    }
                    break;
                case CmdConstant.LoginToServer:
                    {
                        var netProxy=AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
                        netProxy.ConnectToSever();
                    }
                    break;
            }
        }
    }
}

