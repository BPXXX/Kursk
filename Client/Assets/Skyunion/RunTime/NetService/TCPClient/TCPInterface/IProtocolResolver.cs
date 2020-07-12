using System;
using System.IO;

namespace Skyunion
{
    internal interface IProtocolResolver
    {
        /// <summary>
        /// 패킷 파싱
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="bytes"></param>
        /// <param name="packet_size"></param>
        /// <returns></returns>
        MemoryStream PacketProtocolResolve(ArraySegment<byte> segmentBytes, out int packet_size);
    }
}


