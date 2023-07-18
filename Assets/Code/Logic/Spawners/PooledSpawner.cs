using System;
using NTC.Global.System;
using Services.Pools;
using UnityEngine;

namespace Logic.Spawners
{
    public class PooledSpawner<T> : ISpawner<T> where T : IPoolable
    {
        private readonly PoolKey _poolKey;
        private readonly int _preloadCount;
        private readonly Vector3 _disabledPosition = new Vector3(0, -10, 0);
        private readonly IPoolService _poolService;

        private Action _disposeAction = () => { };

        public PooledSpawner(Func<GameObject> poolPreloadFunc, int preloadCount, Func<Action, T, Action> onReturnToPool, IPoolService poolService)
        {
            _poolKey = new PoolKey(typeof(T));
            _preloadCount = preloadCount;
            _poolService = poolService;
            InitPool(poolPreloadFunc, onReturnToPool);
        }

        protected virtual void OnSpawn(T spawned)
        {
        }

        public T Spawn()
        {
            T spawned = _poolService.Get<T>(_poolKey);
            OnSpawn(spawned);
            return spawned;
        }

        public void Dispose() =>
            _disposeAction.Invoke();

        private void InitPool(Func<GameObject> createFunc, Func<Action,T, Action> onReturnToPool)
        {
            _poolService.InstallPool(_poolKey,
                () =>
                {
                    GameObject poolObject = createFunc.Invoke();
                    T pooledComponent = poolObject.GetComponent<T>();
                    _disposeAction += onReturnToPool(() => _poolService.Return(pooledComponent, _poolKey), pooledComponent);
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