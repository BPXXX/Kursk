using System;
using System.IO;

namespace Skyunion
{
    internal class NetworkSyncQueue
    {
        private LockFreeQueue<MemoryStream> mReceivePacketQueue;
        private LockFreeQueue<Const<TCPCommon.NETWORK_STATE>> mStateEventQueue;
        private Object locker = new Object();
        private int m_ReciveCapacityPower = 8;// 256 Size

        public NetworkSyncQueue()
        {
            mReceivePacketQueue = new LockFreeQueue<MemoryStream>(m_ReciveCapacityPower); // 256 Size
            mStateEventQueue = new LockFreeQueue<Const<TCPCommon.NETWORK_STATE>>(2); // 4 Size
        }

        public void PushNetworkStateEvent(Const<TCPCommon.NETWORK_STATE> stateEvent)
        {
            mStateEventQueue.Push(stateEvent);
        }

        public Const<TCPCommon.NETWORK_STATE> PopNetworkStateEvent()
        {
            return mStateEventQueue.Pop();
        }

        public bool HasNetworkStateEvent()
        {
            if (mStateEventQueue.getSize() <= 0)
                return false;

            return true;
        }

        public void PushReceivePacket(MemoryStream packet)
        {
            lock (locker)
            {
                // 因为要队列会超，所以临时扩容
                if(mReceivePacketQueue.isFull())
                {
                    m_ReciveCapacityPower++;
                    var newQueue = new LockFreeQueue<MemoryStream>(m_ReciveCapacityPower); 
                    while(HasReceivePacket())
                    {
                        newQueue.Push(mReceivePacketQueue.Pop());
                    }
                    mReceivePacketQueue = newQueue;
                }
                mReceivePacketQueue.Push(packet);
            }
        }

        public MemoryStream PopReceivePacket()
        {
            MemoryStream stream;
            lock (locker)
            {
                stream = mReceivePacketQueue.Pop();
            }
            return stream;
        }

        public bool HasReceivePacket()
        {
            if (mReceivePacketQueue.getSize() <= 0)
                return false;

            return true;
        }
    }
}
