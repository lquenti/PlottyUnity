using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lquenti
{
    class FixedSizeQueue<T> : IEnumerable<T>
    {
        private LinkedList<T> list = new();
        public int Capacity { get; private set; }
        public int Count { get { return list.Count; } }


        public FixedSizeQueue(int capacity) 
        {
            this.Capacity = capacity;
        }
        public FixedSizeQueue(IEnumerable<T> vals, int capacity) : this(capacity)
        {
            foreach (var x in vals)
            {
                Push(x);
            }
        }

        public void Push(T t)
        {
            list.AddLast(t);
            if (list.Count > Capacity)
            {
                list.RemoveFirst();
            }
        }
        public T Peek()
        {
            return list.Last.Value;
        }
        public T Pop()
        {
            var x = list.Last.Value;
            list.RemoveLast();
            return x;
        }
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= list.Count)
                {
                    throw new ArgumentOutOfRangeException("Out of Index");
                }
                return list.ElementAt(index);
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}