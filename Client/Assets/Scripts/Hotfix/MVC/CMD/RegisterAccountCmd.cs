
using ProtoBuf;
using PureMVC.Interfaces;
using Skyunion;
using System.IO;
using UnityEngine;
namespace Game
{
    public class RegisterAccountCmd : GameCmd
    {
        public override void Execute(INotification notification)
        {
            Msg.Msg_Register_C2S xMsg = new Msg.Msg_Register_C2S();
            xMsg.szName = System.Text.Encoding.Default.GetBytes(TmpData.Data.Account);
            xMsg.szPassword = System.Text.Encoding.Default.GetBytes(TmpData.Data.Passwords);
            MemoryStream stream = new MemoryStream();
            Serializer.Serialize<Msg.Msg_Register_C2S>(stream, xMsg);

            AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.REGISTER_C2S, stream);
        }
    }
}

