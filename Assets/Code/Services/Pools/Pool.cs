using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Pools
{
    public class Pool<T> : IPool where T : IPoolable
    {
        private readonly Func<T> _preloadFunc;
        private readonly Action<T> _getAction;
        private readonly Action<T> _returnAction;
        
        private readonly Transform _parent;

        private readonly Queue<T> _pool = new Queue<T>();
        private readonly List<T> _active = new List<T>();

        public Pool(Func<T> preloadFunc, Action<T> getAction, Action<T> returnAction, int preloadCount, Transform parent = null)
        {
            _preloadFunc = preloadFunc;
            _getAction = getAction;
            _returnAction = returnAction;

            if (preloadFunc is null)
                throw new Exception("Preload function cannot be null");

            _parent = parent ? parent : new GameObject($"Pool {typeof(T).Name}").transform;

            for (int i = 0; i < preloadCount; i++)
            {
                CreatePooled(preloadFunc);
            }
        }

        ~Pool()
        {
            ReturnAll();

            if (_pool.Peek() is ISelfPoolable == false)
                return;
            
            T[] poolArray = _pool.ToArray();

            for (var index = 0; index < poolArray.Length; index++)
            {
                ISelfPoolable pooled = (ISelfPoolable) poolArray[index];
                pooled.Disabled -= Return;
            }
        }

        public T Get()
        {
            T item = _pool.Count > 0 ? _pool.Dequeue() : CreatePooled(_preloadFunc);
            _getAction(item);
            _active.Add(item);

            return item;
        }

        public void Return(T item)
        {
            if (_pool.Contains(item))
            {
#if DEBUG
                Debug.LogWarning("Pool already contains this object");
#endif
                return;
            }

            DefaultReturn(item);
        }

        public void Return(ISelfPoolable item)
        {
            DefaultReturn((T) item);
        }

        private T CreatePooled(Func<T> preloadFunc)
        {
            T pooled = preloadFunc();

            if (pooled is ISelfPoolable selfPoolable)
                selfPoolable.Disabled += Return;

            Return(pooled);
            return pooled;
        }

        private void DefaultReturn(T item)
        {
            item.GameObject.transform.SetParent(_parent, true);
            
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