using System.Threading;

namespace Skyunion
{
    internal class LockFreeQueue<T> where T : class
    {
        private long mHeadPos;
        private long mTailPos;
        private T[] mElements;

        private long mQueueMaxSize;
        private long mQueueSizeMask;

        private long PowerOfTwo(int e)
        {
            if (e == 0)
                return 1;

            return 2 * (PowerOfTwo(e - 1));
        }

        public LockFreeQueue(int capacityPower)
        {
            mQueueMaxSize = PowerOfTwo(capacityPower);
            mQueueSizeMask = mQueueMaxSize - 1;

            mElements = new T[mQueueMaxSize];

            mHeadPos = 0;
            mTailPos = 0;
        }
        public void Push(T newElem)
        {
            long insertPos = Interlocked.Increment(ref mTailPos) - 1;

            mElements[insertPos & mQueueSizeMask] = newElem;
        }
        public T Pop()
        {
            T popVal = Interlocked.Exchange<T>(ref mElements[mHeadPos & mQueueSizeMask], null);

            if (popVal != null)
            {
                Interlocked.Increment(ref mHeadPos);
            }

            return popVal;
        }

        public long getSize()
        {
            return mTailPos - mHeadPos;
        }
        public bool isFull()
        {
            return getSize() >= mQueueSizeMask;
        }
    }
}