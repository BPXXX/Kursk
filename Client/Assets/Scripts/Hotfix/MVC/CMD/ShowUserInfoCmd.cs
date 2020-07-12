
using ProtoBuf;
using PureMVC.Interfaces;
using Skyunion;
using System.IO;
using UnityEngine;
namespace Game
{
    public class ShowUserInfoCmd : GameCmd
    {
        public override void Execute(INotification notification)
        {
            Msg.Msg_GetRoomMen_C2S xMsg = new Msg.Msg_GetRoomMen_C2S();
            xMsg.id = (int)notification.Body;

            MemoryStream stream = new MemoryStream();
            Serializer.Serialize<Msg.Msg_GetRoomMen_C2S>(stream, xMsg);

            AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.GET_ROOMMEN_C2S, stream);

        }
    }
}

