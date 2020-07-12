using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;


namespace Skyunion
{
    internal partial class TCPSession
    {
        public enum SESSION_STATE
        {
            NONE,
            CONNECTED,
            DISCONNECTED,
        }

        private Socket mSocket;
        private SocketAsyncEventArgs mReceiveEventArgs;
        private SocketAsyncEventArgs mSendEventArgs;

        private CircularBuffer mReceiveBuffer = new CircularBuffer(TCPCommon.RECV_BUFFERQUEUE_POWER);
        private CircularBuffer mSendBuffer = new CircularBuffer(TCPCommon.SEND_BUFFERQUEUE_POWER);
        private byte[] mReadQueueBuffer = new byte[TCPCommon.RECV_BUFFER_SIZE+ TCPCommon.PACKET_BUFFER_SIZE];

        private IProtocolResolver mProtocolResolver;
        private WeakReference mClientSession;

        private SESSION_STATE mSessionState;
        public SESSION_STATE sessionState
        {
            get { return mSessionState; }
        }

        private object mSendingBufferLock;

        public void SetClientSession(IClientSession client_session)
        {
            this.mClientSession = new WeakReference(client_session);
        }

        public void SetProtocolResolver(IProtocolResolver protocolResolver)
        {
            this.mProtocolResolver = protocolResolver;
        }
        public TCPSession()
        {
            this.mSendingBufferLock = new object();
            mSessionState = SESSION_STATE.NONE;
        }

        public void ReceiveRequest()
        {
            if (mSessionState != SESSION_STATE.CONNECTED)
                return;

            bool pending = mSocket.ReceiveAsync(mReceiveEventArgs);
            if (false == pending)
            {
                OnReceive();
            }
        }

        public void SendRequest(MemoryStream packet)
        {
            if (mSessionState != SESSION_STATE.CONNECTED)
                return;

            if (packet == null)
                return;

            if (packet == null || packet.Length <= 0)
                return;

            lock (mSendingBufferLock)
            {
                int stored_size = mSendBuffer.GetStoredSize();
                if (false == mSendBuffer.Write(packet.ToArray(), 0, (int)packet.Length))
                {
                }

                if (stored_size <= 0)
                {
                    SendFlush();
                }


            }
        }

        private void SendFlush()
        {
            if (mSessionState != SESSION_STATE.CONNECTED)
                return;

            int storedSize = 0;
            lock (mSendingBufferLock)
            {
                storedSize = mSendBuffer.GetStoredSize();

                mSendBuffer.Peek(mSendEventArgs.Buffer, storedSize);
            }

            mSendEventArgs.SetBuffer(mSendEventArgs.Offset, storedSize);
            bool pending = mSocket.SendAsync(mSendEventArgs);
            if (false == pending)
            {
                OnSend();
            }
        }

        public void OnConnect()
        {
            mSessionState = SESSION_STATE.CONNECTED;

            if (mClientSession.IsAlive)
            {
                (mClientSession.Target as IClientSession).OnConnected();
            }
        }


        public void OnReceive()
        {
            if (mReceiveEventArgs.BytesTransferred > 0 && mReceiveEventArgs.SocketError == SocketError.Success)
            {
                OnProcessReceive(mReceiveEventArgs.Buffer, mReceiveEventArgs.Offset, mReceiveEventArgs.BytesTransferred);

                ReceiveRequest();
            }
            else
            {
                // 连接断开
                //this.Disconnect();
                OnDisconnect();
            }
        }

        public void OnSend()
        {
            if (mSendEventArgs.BytesTransferred <= 0 || mSendEventArgs.SocketError != SocketError.Success)
            {
                return;
            }

            int remainBufferSize = 0;
            lock (mSendingBufferLock)
            {
                mSendBuffer.Remove(mSendEventArgs.BytesTransferred);
                remainBufferSize = mSendBuffer.GetStoredSize();
            }

            if (remainBufferSize > 0)
            {
                SendFlush();
            }

        }

        public void OnDisconnect()
        {
            mSessionState = SESSION_STATE.DISCONNECTED;
            Reset();

            if (mClientSession.IsAlive)
            {
                (mClientSession.Target as IClientSession).OnDisconnected();
            }
        }

        private void OnProcessReceive(byte[] buffer, int offset, int bytes)
        {
            if (false == mReceiveBuffer.Write(buffer, offset, bytes))
            {
                Disconnect();
            }

            int storedSize = mReceiveBuffer.GetStoredSize();
            int offsetIndex = 0;

            if (false == mReceiveBuffer.Peek(mReadQueueBuffer, storedSize))
            {

            }

            MemoryStream packet = null;
            int packetSize = 0;
            while ((packet = mProtocolResolver.PacketProtocolResolve(new ArraySegment<byte>(mReadQueueBuffer, offsetIndex, storedSize), out packetSize)) != null)
            {
                if (false == mClientSession.IsAlive)
                {
                    break;
                }
                (mClientSession.Target as IClientSession).OnRead(packet);

                offsetIndex += packetSize;
                storedSize -= packetSize;

                mReceiveBuffer.Remove(packetSize);

                if (storedSize <= 0)
                {
                    break;
                }
            }

        }

        public void Reset()
        {
            Debug.Log("网络数据重置");
            mSendBuffer.Clear();
            mReceiveBuffer.Clear();
        }

        public void Disconnect()
        {
            mSessionState = SESSION_STATE.DISCONNECTED;
            try
            {
                mSocket.Shutdown(SocketShutdown.Send);
            }
            catch (Exception) { }
            mSocket.Close();
            mSocket = null;

            //> 临时添加
            OnDisconnect();
        }
    }

}

