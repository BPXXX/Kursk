using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Skyunion;

namespace Skyunion
{
    public enum NetEvent
    {
        ConnectComplete,
        DisconnectedComplete,
        ConnectFail,
        ReconnectFail,
        AuthFail
    }

    public interface INetClient
    {
        void Connect(string host, int port);
        void Disconnect();
        void Reconnect();
        void Send(MemoryStream packet);
    }
    public class NetPackInfo
    {
        // 整包大小(包头+内容)
        public int packet_size;
        // 内容
        public MemoryStream content;
    }
    public delegate NetPackInfo ProtocolResolverDelegate(ArraySegment<byte> segmentBytes);
    public interface INetServcice : IModule
    {
        INetClient CreateClient(Action<NetEvent,int> connectEvent, Action<MemoryStream> reciveEvent, ProtocolResolverDelegate protocolResolver);
    }
}
