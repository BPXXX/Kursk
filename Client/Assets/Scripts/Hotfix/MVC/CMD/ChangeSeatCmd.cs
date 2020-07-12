
using ProtoBuf;
using PureMVC.Interfaces;
using Skyunion;
using System.IO;
using UnityEngine;
namespace Game
{
    public class ChangeSeatCmd : GameCmd
    {
        public override void Execute(INotification notification)
        {
            Msg.Msg_ChangeState_C2S xMsg = new Msg.Msg_ChangeState_C2S();
            xMsg = notification.Body as Msg.Msg_ChangeState_C2S;

            MemoryStream stream = new MemoryStream();
            Serializer.Serialize<Msg.Msg_ChangeState_C2S>(stream, xMsg);

            AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.CHANGE_STATE_C2S, stream);

        }
    }
}

