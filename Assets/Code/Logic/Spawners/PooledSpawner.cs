using System;
using NTC.Global.System;
using UnityEngine;
using Pool;

namespace Logic.Spawners
{
    public class PooledSpawner<T> : ISpawner<T> where T : Component
    {
        private readonly int _preloadCount;
        
        private Pool<T> _pool;
        private Action _disposeAction = () => { };

        public PooledSpawner(Func<GameObject> poolPreloadFunc, int preloadCount, Func<Action, T, Action> onReturnToPool)
        {
            _preloadCount = preloadCount;
            InitPool(poolPreloadFunc, onReturnToPool);
        }

        protected virtual void OnSpawn(T spawned)
        {
        }

        protected virtual void OnReturn(T agent)
        {
        }

        protected virtual void OnGet(T agent)
        {
        }

        public T Spawn()
        {
            T spawned = _pool.Get();
            OnSpawn(spawned);
            return spawned;
        }

        public void Dispose() =>
            _disposeAction.Invoke();

        private void InitPool(Func<GameObject> poolPreloadFunc, Func<Action,T, Action> onReturnToPool)
        {
            _pool = new Pool<T>(
                () =>
                {
                    GameObject poolObject = poolPreloadFunc.Invoke();
                    T pooledComponent = poolObject.GetComponent<T>();
                    _disposeAction += onReturnToPool(() => _pool.Return(pooledComponent), pooledComponent);
                    return pooledComponent;
                },
                GetAction,
                ReturnAction,
                _preloadCount
            );
        }

        private void ReturnAction(T obj)
        {
            obj.gameObject.Disable();
            OnReturn(obj);
        }

        private void GetAction(T obj)
        {
            obj.gameObject.Enable();
            OnGet(obj);
        }
    }
}