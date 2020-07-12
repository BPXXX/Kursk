using System;
using System.Net.Sockets;
using System.Net;
using UnityEngine;


namespace Skyunion
{
    internal class TCPConnector
    {
        public delegate void ConnectHandler(TCPSession tcpSession);
        public delegate void FailHandler(int errorCode);

        private Socket mSocket;
        private IPEndPoint mEndPoint;
        private ConnectHandler mConnectedCallback;
        private FailHandler mConnectedFailCallback;

        public ConnectHandler connectHandler
        {
            set { mConnectedCallback = value; }
        }

        public FailHandler failHandler
        {
            set { mConnectedFailCallback = value; }
        }

        public bool Connect(String dnsHost, int port, IProtocolResolver protocol_resolver, TCPSession tcpSession = null)
        {
            
            
            IPAddress address = null;
            if (!IPAddress.TryParse(dnsHost, out address))
            {
                try
                {
                    var addressDns = Dns.GetHostAddresses(dnsHost);
                    address = addressDns[0];
                }
                catch (Exception e)
                {
                   Debug.LogWarning("域名解析错误"+dnsHost);
                   
                   this.mConnectedFailCallback?.Invoke(101);
                   return false;
                }
            }
            
            this.mEndPoint = new IPEndPoint(address, port);
            this.mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (tcpSession == null)
            {
                tcpSession = new TCPSession();
            }

            tcpSession.SetProtocolResolver(protocol_resolver);

            SocketAsyncEventArgs event_arg = new SocketAsyncEventArgs();
            event_arg.Completed += OnConnect;
            event_arg.RemoteEndPoint = mEndPoint;
            event_arg.UserToken = tcpSession;
//            bool pending = mSocket.ConnectAsync(event_arg);
//            if (false == pending)
//            {
//                OnConnect(null, event_arg);
//            }
//            
            //fix zj
            System.Threading.ManualResetEvent mre = new System.Threading.ManualResetEvent(false);
            IAsyncResult result = mSocket.BeginConnect(mEndPoint, (res) => mre.Set(), null);
            bool active = mre.WaitOne(TimeSpan.FromSeconds(TCPCommon.CONNECT_TIMEOUT));
            if (active)
            {
                try
                {
                    mSocket.EndConnect(result);
                    OnConnect(null, event_arg);
                }
                catch (Exception e)
                {
                    this.mConnectedFailCallback?.Invoke(107);
                }
            }
            else
            {
                try
                {
                    mSocket.Disconnect(false);
                }
                catch  (Exception e)
                {
                    
                }
                this.mConnectedFailCallback?.Invoke(999);

                Debug.Log("socket connection timed out!");
            }

            return true;
        }
        private void OnConnect(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                TCPSession tcpSession = e.UserToken as TCPSession;
                tcpSession.OnConnectCompleted(mSocket);

                this.mConnectedCallback?.Invoke(tcpSession);

                tcpSession.OnConnect();
                tcpSession.ReceiveRequest();
            }
            else
            {
                this.mConnectedFailCallback?.Invoke(-1);
            }
        }
    }
}

