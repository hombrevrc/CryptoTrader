﻿using System.Collections.Generic;

namespace CryptoTrading.Logic.Utils
{
    public class FixedSizedQueue<T>
    {
        private readonly Queue<T> _queue;
        private readonly int _size;

        public FixedSizedQueue(int size)
        {
            _size = size;
            _queue = new Queue<T>(size);
        }

        public void Enqueue(T obj)
        {
            if (_queue.Count < _size)
            {
                _queue.Enqueue(obj);
            }
            else
            {
                _queue.Dequeue();
                _queue.Enqueue(obj);
            }
        }

        public int QueueSize => _queue.Count;

        public T this[int i]
        {
            get
            {
                if (i < 0 || i > _queue.Count)
                {
                    return default(T);
                }
                return _queue.ToArray()[i];
            }
        }
    }
}