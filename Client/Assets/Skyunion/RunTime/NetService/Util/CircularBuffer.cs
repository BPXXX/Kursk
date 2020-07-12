using System;

namespace Skyunion
{
    internal class CircularBuffer
    {
        private byte[] mBuffer = null;
        private int mCapacity = 0;
        private int mCapacityMask = 0;
        private int mHead = 0;
        private int mTail = 0;

        private int PowerOfTwo(int e)
        {
            if (e == 0)
                return 1;

            return 2 * (PowerOfTwo(e - 1));
        }

        public CircularBuffer(int capacityPower)
        {
            mCapacity = PowerOfTwo(capacityPower);
            mCapacityMask = mCapacity - 1;
            mBuffer = new byte[mCapacity];

        }

        public WeakReference GetBuffer()
        {
            return new WeakReference(mBuffer);
        }

        public int GetStoredSize()
        {
            if (mHead > mTail)
            {
                return mHead - mTail;
            }
            else if (mHead < mTail)
            {
                return (mCapacity - mTail) + mHead;
            }

            return 0;
        }

        public bool Peek(byte[] destBuf, int bytes)
        {
            if (bytes > GetStoredSize())
                return false;

            int readOffset = mTail + bytes;
            int afterReadBytes = readOffset > mCapacity ? readOffset & mCapacityMask : 0;
            int readBytes = bytes - afterReadBytes;
            Buffer.BlockCopy(mBuffer, mTail, destBuf, 0, readBytes);

            if (afterReadBytes > 0)
            {
                Buffer.BlockCopy(mBuffer, 0, destBuf, readBytes, afterReadBytes);
            }

            return true;
        }

        public bool Read(byte[] destBuf, int bytes)
        {
            if (bytes > GetStoredSize())
                return false;

            int readOffset = mTail + bytes;
            int afterReadBytes = readOffset > mCapacity ? readOffset & mCapacityMask : 0;
            int readBytes = bytes - afterReadBytes;
            Buffer.BlockCopy(mBuffer, mTail, destBuf, 0, readBytes);

            if (afterReadBytes > 0)
            {
                Buffer.BlockCopy(mBuffer, 0, destBuf, readBytes, afterReadBytes);
            }

            mTail = readOffset & mCapacityMask;

            return true;
        }



        public bool Write(byte[] data, int offset, int bytes)
        {
            if (mCapacity < GetStoredSize() + bytes)
            {
                return false;
            }

            int writeOffset = mHead + bytes;

            int afterWriteBytes = writeOffset > mCapacity ? writeOffset & mCapacityMask : 0;

            int writeBytes = bytes - afterWriteBytes;
            Buffer.BlockCopy(data, offset, mBuffer, mHead, writeBytes);

            if (afterWriteBytes > 0)
            {
                Buffer.BlockCopy(data, offset + writeBytes, mBuffer, 0, afterWriteBytes);
            }

            mHead = writeOffset & mCapacityMask;

            return true;

        }

        public bool Remove(int bytes)
        {
            if (bytes > GetStoredSize())
                return false;

            mTail = (mTail + bytes) & mCapacityMask;

            return true;
        }

        public void Clear()
        {
            mHead = 0;
            mTail = 0;
        }
    }
}
