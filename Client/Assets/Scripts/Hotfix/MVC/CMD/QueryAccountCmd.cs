
using ProtoBuf;
using PureMVC.Interfaces;
using Skyunion;
using System.IO;
using UnityEngine;
namespace Game
{
    public class QueryAccountCmd : GameCmd
    {
        public override void Execute(INotification notification)
        {
            Msg.Msg_Login_C2S xMsg = new Msg.Msg_Login_C2S();
            xMsg.szName = System.Text.Encoding.Default.GetBytes(TmpData.Data.Account);
            xMsg.szPassword = System.Text.Encoding.Default.GetBytes(TmpData.Data.Passwords);
            MemoryStream stream = new MemoryStream();
            Serializer.Serialize<Msg.Msg_Login_C2S>(stream, xMsg);

            AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.LOGIN_C2S, stream);
        }
    }
}

