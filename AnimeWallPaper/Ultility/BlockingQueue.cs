using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace AnimeWallPaper.Ultility
{
    public class BlockingQueue<T>
    {
        private const int MaxWaitingCount = 1;
        private Queue<T> q = new Queue<T>();
        public bool ClearQueue { get; set; }

        public BlockingQueue(bool clearQueue = false)
        {
            ClearQueue = clearQueue;
        }

        public void Add(T element)
        {
            lock (q)
            {
                if (q.Count > MaxWaitingCount && ClearQueue)
                {
                    Debug.WriteLine("clear queue");
                    q.Clear();
                }
                q.Enqueue(element);
                Monitor.PulseAll(q);
            }
        }

        public T Get()
        {
            lock (q)
            {
                while (IsEmpty())
                {
                    Monitor.Wait(q);
                }
                return q.Dequeue();
            }
        }

        private bool IsEmpty()
        {
            lock (q)
            {
                return q.Count == 0;
            }
        }

        public void ClearAll()
        {
            lock (q)
            {
                q.Clear();
            }
        }
    }
}
