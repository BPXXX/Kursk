using System.IO;
using System;
using UnityEngine;

namespace Skyunion
{
    internal class NetClient : INetClient
    {
        private NetworkManager mNetworkManager;
        private IProtocolResolver mProtocolResolver;

        public NetClient(Action<NetEvent,int> connectEvent, Action<MemoryStream> reciveEvent, IProtocolResolver protocolResolver)
        {
            mNetworkManager = new NetworkManager();

            mNetworkManager.mReceiveEventCallback += (MemoryStream pack) =>
            {
                reciveEvent?.Invoke(pack);
            };
            mNetworkManager.mConnectCompleteEvent += (errorCode) =>
            {
                connectEvent?.Invoke(NetEvent.ConnectComplete,errorCode);
                
            };
            mNetworkManager.mDisconnectedCompleteEvent += (errorCode) =>
            {
                connectEvent?.Invoke(NetEvent.DisconnectedComplete,errorCode);
            };
            mNetworkManager.mConnectFailEvent += (errorCode) =>
            {
                connectEvent?.Invoke(NetEvent.ConnectFail,errorCode);
            };
            mNetworkManager.mReconnectFailEvent += (errorCode) =>
            {
                mNetworkManager.SwitchStateHandle(TCPCommon.NETWORK_STATE.NONE);
                connectEvent?.Invoke(NetEvent.ReconnectFail,errorCode);
            };
            mProtocolResolver = protocolResolver;
        }

        public void Connect(string host, int port)
        {
            mNetworkManager.Connect(host, port, mProtocolResolver);
        }

        public void Send(MemoryStream stream)
        {
            mNetworkManager.Send(stream);
        }

        public void Disconnect()
        {
            mNetworkManager.Disconnect();
        }
        public void Reconnect()
        {
            mNetworkManager.Reconnect();
        }

        public void Update()
        {
            mNetworkManager.Update();
        }
    }
}
