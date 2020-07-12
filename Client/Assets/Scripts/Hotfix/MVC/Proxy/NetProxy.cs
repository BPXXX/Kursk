// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年11月8日
// Update Time         :    2019年11月8日
// Class Description   :    NetProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using Skyunion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Game
{
    public class NetProxy : GameProxy
    {

        #region Member
        public const string ProxyNAME = "NetProxy";

        private INetServcice netService;
        private ILogService logService;
        private INetClient netClient;
        private NetPackInfo netPackInfo = new NetPackInfo();

        #endregion

        // Use this for initialization
        public NetProxy(string proxyName)
            : base(proxyName)
        {

        }

        public NetPackInfo PacketProtocolResolve(ArraySegment<byte> segmentBytes)
        {
            netPackInfo.packet_size = 0;
            if (segmentBytes.Count < 2 + 2)  //pack size + msgno size
            {
                return null;
            }

            byte[] recv = segmentBytes.Array;
            ushort bodyLen = BitConverter.ToUInt16(recv,segmentBytes.Offset);
            // Body 大小
            //UInt16 bodyLen = BitConverter.ToUInt16(segmentBytes.Array, segmentBytes.Offset);
            netPackInfo.packet_size = bodyLen;

            // (信息包尺寸)
            //packetSize = bodyLen + ClientCommon.HEADER_SIZE;
            // Body
            if (segmentBytes.Count < netPackInfo.packet_size)
            {
                return null;
            }
            byte[] packetData = new byte[bodyLen];
            Array.Copy(segmentBytes.Array, segmentBytes.Offset, packetData, 0, bodyLen);
            netPackInfo.content = new MemoryStream();
            netPackInfo.content.Write(packetData, 0, bodyLen);
            return netPackInfo;
        }

        public override void OnRegister()
        {
            Debug.Log(" NetProxy register");
        }

        public override void OnRemove()
        {
            Debug.Log(" NetProxy remove");
        }

        private void OnNetEvent(NetEvent @event, int error)
        {
            SendNotification(CmdConstant.ConnectEvent, @event);
        }

        // 收到单个包体的二进制数据， 需要根据你是  ProtocolBuf或者什么协议解析成你的类 再通过 
        // AppFacade.GetInstance().SendNotification(msgID, body, type); 派发出去
        private void OnReciveEvent(MemoryStream packt)
        {
            var msgID = BitConverter.ToUInt16(packt.GetBuffer(), 2);
            Debug.Log("OnMsgRecive:" + msgID.ToString());
            AppFacade.GetInstance().SendNotification(msgID.ToString(), new MemoryStream(packt.GetBuffer(), 4, (int)packt.Length - 4, false), "msg");
        }

        public void SendPBMsg(UInt16 id, MemoryStream stream)
        {
            MemoryStream pack = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(pack);
            ushort msgLen = (ushort)(stream.Length + 4);
            writer.Write(msgLen);
            writer.Write((UInt16)id);
            stream.WriteTo(pack);
            netClient.Send(pack);
        }
        public void ConnectToSever()
        {
            netService = PluginManager.Instance().FindModule<INetServcice>();
            logService = PluginManager.Instance().FindModule<ILogService>();
            if(netClient==null)
            {
                netClient = netService.CreateClient(OnNetEvent, OnReciveEvent, PacketProtocolResolve);
                netClient.Connect("127.0.0.1", 3000);
            }
            
        }
        public void CloseSever()
        {
            if(netClient!=null)
            {
                netClient.Disconnect();
                netClient = null;
            }
        }
    }
}