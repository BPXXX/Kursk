using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace Skyunion
{
    internal interface IClientSession
    {
        TCPSession TcpSession { get; }
        void OnRead(MemoryStream packet);
        void OnConnected();
        void OnDisconnected();
        void Send(MemoryStream packet);
        void Disconnect();
    }
}
