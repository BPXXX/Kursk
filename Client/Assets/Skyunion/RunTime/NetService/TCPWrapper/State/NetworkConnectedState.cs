using System;
using System.IO;
using UnityEngine;

namespace Skyunion
{
    internal class NetworkConnectedState : INetworkState
    {
        private NetworkManager mNetworkManager = null;
        private IClientSession mClientSession = null;
        private String mHost;
        private int mPort;
        private IProtocolResolver mProcotolResolver = null;

        public NetworkConnectedState(String host, int port, IProtocolResolver protocolResolver, IClientSession clientSession)
        {
            this.mHost = host;
            this.mPort = port;
            this.mProcotolResolver = protocolResolver;
            this.mClientSession = clientSession;
        }

        public void Enter(NetworkManager networkManager)
        {
            networkManager.OnConnectEventFire();
        }

        public void Connect(string host, int port, IProtocolResolver protocolResolver)
        {
			
        }

        public void Disconnect()
        {
            mClientSession.Disconnect();
        }

        public void Reconnect()
        {
            mClientSession.Disconnect();
            
            TCPConnector connector = new TCPConnector();
            connector.connectHandler = mNetworkManager.OnReconnectComplete;
            connector.failHandler = mNetworkManager.OnReconnectFail;
            connector.Connect(mHost, mPort, mProcotolResolver, mClientSession.TcpSession);
        }

        public void Send(MemoryStream packet)
        {
            //Debug.Log("链接状态下发包");
            if (mClientSession == null)
                return;

            mClientSession.Send(packet);
        }


    }
}
