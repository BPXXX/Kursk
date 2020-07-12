using System;
using System.Net.Sockets;


namespace Skyunion
{
    internal partial class TCPSession
    {
        public void OnConnectCompleted(Socket socket)
        {
            if (sessionState == SESSION_STATE.NONE)
            {
                // Receive Socket Async Event Regist
                mReceiveEventArgs = new SocketAsyncEventArgs();
                mReceiveEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnReceiveCompleted);
                mReceiveEventArgs.UserToken = this;
                mReceiveEventArgs.SetBuffer(new byte[TCPCommon.RECV_BUFFER_SIZE], 0, TCPCommon.RECV_BUFFER_SIZE);

                // Send Socket Async Event Regist
                mSendEventArgs = new SocketAsyncEventArgs();
                mSendEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);
                mSendEventArgs.UserToken = this;
                mSendEventArgs.SetBuffer(new byte[TCPCommon.SEND_BUFFER_SIZE], 0, TCPCommon.SEND_BUFFER_SIZE);
            }

            mSocket = socket;
        }

        public void OnReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            TCPSession tcpSession = e.UserToken as TCPSession;
            if (e.LastOperation == SocketAsyncOperation.Receive)
            {
                tcpSession.OnReceive();
                return;
            }
            else
            {
                tcpSession.OnDisconnect();
            }
        }

        public void OnSendCompleted(object sender, SocketAsyncEventArgs e)
        {
            TCPSession tcpSession = e.UserToken as TCPSession;
            tcpSession.OnSend();
        }
    }


}
