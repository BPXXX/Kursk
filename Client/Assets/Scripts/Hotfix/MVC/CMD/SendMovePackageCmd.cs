
using ProtoBuf;
using PureMVC.Interfaces;
using Skyunion;
using System.IO;
using UnityEngine;
namespace Game
{
    public class SendMovePackageCmd : GameCmd
    {
        public override void Execute(INotification notification)
        {
            Debug.Log("发送Move包");
            Msg.Msg_VEC_C2S xMsg = new Msg.Msg_VEC_C2S();
            xMsg.direction = 1;
            xMsg.x = 1;
            xMsg.y = 2;
            xMsg.ingrass = 1;
            


            MemoryStream stream = new MemoryStream();
            Serializer.Serialize<Msg.Msg_VEC_C2S>(stream, xMsg);

            AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.VEC_C2S, stream);
        }
    }
}

