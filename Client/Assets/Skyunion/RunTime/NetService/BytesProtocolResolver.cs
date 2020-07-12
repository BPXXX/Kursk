using System;
using System.IO;

namespace Skyunion
{
    internal class ProtocolResolverCallback : IProtocolResolver
    {
        private ProtocolResolverDelegate callback;
        public ProtocolResolverCallback(ProtocolResolverDelegate protocolResolver)
        {
            callback = protocolResolver;
        }
        public MemoryStream PacketProtocolResolve(ArraySegment<byte> segmentBytes, out int packetSize)
        {
            var netPackage = callback(segmentBytes);
            if(netPackage == null)
            {
                packetSize = 0;
                return null;
            }
            packetSize = netPackage.packet_size;
            return netPackage.content;
        }
    }

    internal class BytesProtocolResolver : IProtocolResolver
    {
        MemoryStream IProtocolResolver.PacketProtocolResolve(ArraySegment<byte> segmentBytes, out int packetSize)
        {
            packetSize = 0;

            if (segmentBytes.Count < 2 + 4)  //pack size + msgno size
            {
                return null;
            }

            byte[] recv = segmentBytes.Array;
            ushort bodyLen = (ushort)(recv[0] << 8 | recv[1]);
            // Body 大小
            //UInt16 bodyLen = BitConverter.ToUInt16(segmentBytes.Array, segmentBytes.Offset);
            packetSize = 2 + bodyLen;

            // (信息包尺寸)
            //packetSize = bodyLen + ClientCommon.HEADER_SIZE;
            // Body
            if (segmentBytes.Count < packetSize)
            {
                return null;
            }
            byte[] packetData = new byte[bodyLen];
            int copyOffset = segmentBytes.Offset + 2;
            Array.Copy(segmentBytes.Array, copyOffset, packetData, 0, bodyLen);
            MemoryStream packet = new MemoryStream(packetData, 0, packetData.Length);

            return packet;
        }
    }
}