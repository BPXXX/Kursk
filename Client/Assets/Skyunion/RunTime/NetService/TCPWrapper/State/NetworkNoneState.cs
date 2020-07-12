﻿
using System.IO;
using UnityEngine;

namespace Skyunion
{
    internal class NetworkNoneState : INetworkState
    {
        private NetworkManager mNetworkManager;

        public void Enter(NetworkManager networkManager)
        {
            this.mNetworkManager = networkManager;
        }

        public void Connect(string host, int port, IProtocolResolver protocolResolver)
        {
            mNetworkManager.SettingConnector(host, port, protocolResolver);

            TCPConnector connector = new TCPConnector();
            connector.connectHandler = mNetworkManager.OnConnnectComplete;
            connector.failHandler = mNetworkManager.OnConnectFail;
            connector.Connect(host, port, protocolResolver);
        }

        public void Disconnect()
        {

        }

        public void Reconnect()
        {
        }

        public void Send(MemoryStream packet)
        {
            Debug.Log("断网了不能发包");
        }


    }
}
