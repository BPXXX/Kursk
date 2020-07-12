
using ProtoBuf;
using PureMVC.Interfaces;
using Skyunion;
using System.IO;
using UnityEngine;
namespace Game
{
    public class StartWarCmd : GameCmd
    {
        public override void Execute(INotification notification)
        {
            Msg.Msg_StartGame_C2S xMsg = new Msg.Msg_StartGame_C2S();
            xMsg.id = UI_CreatRoomPanelMediator.CreateRoomid;
            MemoryStream stream = new MemoryStream();
            Serializer.Serialize<Msg.Msg_StartGame_C2S>(stream, xMsg);

            Debug.Log("发送了Msg_StartGame_C2S");
            AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.START_GAME_C2S, stream);
        }
    }
}

