namespace Skyunion
{
    internal class TCPCommon
    {
        public enum NETWORK_STATE : byte
        {
            CONNECT,
            DISCONNECT,
            NONE
        };

        public static readonly int SEND_BUFFERQUEUE_POWER = 17;

        public static readonly int RECV_BUFFERQUEUE_POWER = 17;

        public static readonly int SEND_BUFFER_SIZE = 65536;
        public static readonly int RECV_BUFFER_SIZE = 65536;
        public static readonly int PACKET_BUFFER_SIZE = 65536;
        public static readonly int PACKET_PROCESS_PERFRAME = 10;

        public static readonly int CONNECT_TIMEOUT = 10;


        public static int ErrorDNS = 101;

        public static int ErrorConnectTimeout = 199;
        

    }
}
