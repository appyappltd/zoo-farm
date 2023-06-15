using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public class Pool<T>
    {
        private readonly Func<T> _preloadFunc;
        private readonly Action<T> _getAction;
        private readonly Action<T> _returnAction;

        private readonly Queue<T> _pool = new Queue<T>();
        private readonly List<T> _active = new List<T>();

        public Pool(Func<T> preloadFunc, Action<T> getAction, Action<T> returnAction, int preloadCount)
        {
            _preloadFunc = preloadFunc;
            _getAction = getAction;
            _returnAction = returnAction;

            if (preloadFunc is null)
                throw new Exception("Preload function cannot be null");

            for (int i = 0; i < preloadCount; i++)
                Return(preloadFunc());
        }

        public T Get()
        {
            T item = _pool.Count > 0 ? _pool.Dequeue() : _preloadFunc();
            Debug.Log(_pool.Count);
            _getAction(item);
            _active.Add(item);

            return item;
        }

        public void Return(T item)
        {
            _returnAction(item);
            _pool.Enqueue(item);
            _active.Remove(item);
        }

        public void ReturnAll()
        {
            for (int i = 0; i < _active.Count; i++)
                Return(_active[i]);
        }
    }
}