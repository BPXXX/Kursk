
using PureMVC.Interfaces;
using Skyunion;
using UnityEngine;
namespace Game
{
    public class StartupCommand :GameCmd
    {
        public override void Execute(INotification notification)
        {
            SendNotification(CmdConstant.ReloadGame);
        }
    }
}

