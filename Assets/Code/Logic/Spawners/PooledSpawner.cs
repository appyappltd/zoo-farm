using System;
using NTC.Global.System;
using Services.Pools;
using UnityEngine;

namespace Logic.Spawners
{
    public class PooledSpawner<T> : ISpawner<T> where T : IPoolable
    {
        private readonly int _preloadCount;
        private readonly Vector3 _disabledPosition = new Vector3(0, -10, 0);
        private readonly IPoolService _poolService;
        
        private Action _disposeAction = () => { };

        public PooledSpawner(Func<GameObject> poolPreloadFunc, int preloadCount, Func<Action, T, Action> onReturnToPool, IPoolService poolService)
        {
            _preloadCount = preloadCount;
            _poolService = poolService;
            InitPool(poolPreloadFunc, onReturnToPool);
        }

        protected virtual void OnSpawn(T spawned)
        {
        }

        public T Spawn()
        {
            T spawned = _poolService.Get<T>();
            OnSpawn(spawned);
            return spawned;
        }

        public void Dispose() =>
            _disposeAction.Invoke();

        private void InitPool(Func<GameObject> createFunc, Func<Action,T, Action> onReturnToPool)
        {
            _poolService.InstallPool(() =>
                {
                    GameObject poolObject = createFunc.Invoke();
                    T pooledComponent = poolObject.GetComponent<T>();
                    _disposeAction += onReturnToPool(() => _poolService.Return(pooledComponent), pooledComponent);
                    return pooledComponent;
                },
                GetAction,
                ReturnAction,
                _preloadCount);
        }

        private void ReturnAction(T obj)
        {
            obj.GameObject.Disable();
            obj.GameObject.transform.localPosition = _disabledPosition;
        }

        private void GetAction(T obj)
        {
            obj.GameObject.Enable();
        }
    }
}