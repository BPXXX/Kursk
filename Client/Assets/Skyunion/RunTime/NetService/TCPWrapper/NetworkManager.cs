using UnityEngine;
using System;
using System.IO;

namespace Skyunion
{
    internal class NetworkManager
    {
        public delegate void NetworkEventCallback(int errorCode=0);
        public delegate void ReceiveEventCallback(MemoryStream pack);

        public event NetworkEventCallback mConnectFailEvent;
        public event NetworkEventCallback mReconnectFailEvent;
        public event NetworkEventCallback mConnectCompleteEvent;
        public event NetworkEventCallback mDisconnectedCompleteEvent;
        public ReceiveEventCallback mReceiveEventCallback;

        private INetworkState mNetworkState = null;
        private NetworkSyncQueue mNetworkSyncQueue = null;
        private IClientSession mClientSession = null;
        private IProtocolResolver mProtocolResolver = null;
        private String mIp;
        private int mPort;

        private TCPCommon.NETWORK_STATE mTCPState;
        public TCPCommon.NETWORK_STATE tcpState
        {
            get { return mTCPState; }
        }

        public void SettingConnector(string host, int port, IProtocolResolver protocolResolver)
        {
            this.mIp = host;
            this.mPort = port;
            this.mProtocolResolver = protocolResolver;
        }

        public void Connect(string host, int port, IProtocolResolver protocolResolver)
        {
            SwitchStateHandle(TCPCommon.NETWORK_STATE.NONE);
            mNetworkState.Connect(host, port, protocolResolver);
        }
        public void Disconnect()
        {
            Debug.Log("网络断开 :"+mNetworkState);
            mNetworkState.Disconnect();
        }
        public void Reconnect()
        {
            if (mTCPState == TCPCommon.NETWORK_STATE.CONNECT)
            {
                Disconnect();
                Update();
            }
            Debug.Log("网络重连 :"+mNetworkState);
            mNetworkState.Reconnect();
        }
        public void SwitchStateHandle(TCPCommon.NETWORK_STATE state)
        {
            if (mTCPState == state)
                return;
            
            
            Debug.Log("切换网络处理方式:"+state);

            mTCPState = state;
            switch (mTCPState)
            {
                case TCPCommon.NETWORK_STATE.CONNECT:
                    {
                        mNetworkState = new NetworkConnectedState(mIp, mPort, mProtocolResolver, mClientSession);
                    }
                    break;

                case TCPCommon.NETWORK_STATE.DISCONNECT:
                    {
                        mNetworkState = new NetworkDisconnectedState(mIp, mPort, mProtocolResolver, mClientSession);
                    }
                    break;

                case TCPCommon.NETWORK_STATE.NONE:
                    {
                        mNetworkState = new NetworkNoneState();
                    }
                    break;

                default:
                    return;

            }

            mNetworkState.Enter(this);
        }
        public void Send(MemoryStream packet)
        {
            mNetworkState.Send(packet);
        }
        #region CALLBACK
        public void OnConnnectComplete(TCPSession tcpSession)
        {
            mNetworkSyncQueue = new NetworkSyncQueue();
            mClientSession = new ClientSession(tcpSession, mNetworkSyncQueue, mProtocolResolver);
        }
        public void OnConnectFail(int errorCode)
        {
            if (mConnectFailEvent != null)
            {
                mConnectFailEvent(errorCode);
            }
        }
        public void OnReconnectComplete(TCPSession tcpSession)
        {
            
        }
        public void OnReconnectFail(int errorCode)
        {
            if (mReconnectFailEvent != null)
            {
                mReconnectFailEvent(errorCode);
            }
        }
        private void OnReceive(MemoryStream packet)
        {
            mReceiveEventCallback(packet);
        }
        public void OnConnectEventFire()
        {
            if(mConnectCompleteEvent != null)
                mConnectCompleteEvent();
        }
        public void OnDisconnectEventFire()
        {
            if(mDisconnectedCompleteEvent != null)
                mDisconnectedCompleteEvent();
        }
        #endregion

        public void Update()
        {
            if (mNetworkSyncQueue == null)
            {
                return;
            }
            if (this.mNetworkSyncQueue.HasNetworkStateEvent())
            {
                Const<TCPCommon.NETWORK_STATE> constNetworkState = this.mNetworkSyncQueue.PopNetworkStateEvent();
                if (constNetworkState != null)
                {
                    SwitchStateHandle(constNetworkState.Value);
                }
            }
            //暂时注释掉  网络断掉的时候最后一个包不会收到
            //            if (mTCPState != TCPCommon.NETWORK_STATE.CONNECT)
            //                return;

            int prcess = 0;
            while (this.mNetworkSyncQueue.HasReceivePacket())
            {
                MemoryStream packet = this.mNetworkSyncQueue.PopReceivePacket();
                if (packet != null)
                {
                    OnReceive(packet);
                }
                prcess++;
                if(prcess == TCPCommon.PACKET_PROCESS_PERFRAME)
                {
                    break;
                }
            }
        }
    }
}
