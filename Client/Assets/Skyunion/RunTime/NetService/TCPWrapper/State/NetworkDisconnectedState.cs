using System;
using System.IO;
using UnityEngine;

namespace Skyunion
{
    internal class NetworkDisconnectedState : INetworkState
    {
        private NetworkManager mNetworkManager = null;
        private String mHost;
        private int mPort;
        private IProtocolResolver mProcotolResolver = null;
        private IClientSession mClientSession = null;

        public NetworkDisconnectedState(String host, int port, IProtocolResolver protocolResolver, IClientSession clientSession)
        {
            this.mHost = host;
            this.mPort = port;
            this.mProcotolResolver = protocolResolver;
            this.mClientSession = clientSession;
        }

        public void Enter(NetworkManager networkManager)
        {
            this.mNetworkManager = networkManager;

            mNetworkManager.OnDisconnectEventFire();
        }

        public void Connect(string host, int port, IProtocolResolver protocolResolver)
        {
        }

        public void Disconnect()
        {

        }

        public void Reconnect()
        {
            TCPConnector connector = new TCPConnector();
            connector.connectHandler = mNetworkManager.OnReconnectComplete;
            connector.failHandler = mNetworkManager.OnReconnectFail;
            connector.Connect(mHost, mPort, mProcotolResolver, mClientSession.TcpSession);
        }

        public void Send(MemoryStream packet)
        {
            Debug.Log("断网下不能发包");
        }

    }
}
