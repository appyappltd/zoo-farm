using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Services.Pools
{
    public class PoolService : IPoolService
    {
        private readonly Dictionary<Type, IPool> _allPools = new Dictionary<Type, IPool>(16);

        public void InstallPool<TPooled>(Func<TPooled> preloadFunc, Action<TPooled> getAction,
            Action<TPooled> returnAction, int preloadCount) where TPooled : IPoolable
        {
            if (_allPools.ContainsKey(typeof(TPooled)))
            {
#if DEBUG
                Debug.LogWarning($"Pool with type {nameof(TPooled)} already exists");
#endif
                return;
            }

            Pool<TPooled> newPool = new Pool<TPooled>(preloadFunc, getAction, returnAction, preloadCount);
            _allPools.Add(typeof(TPooled), newPool);
        }

        public TPooled Get<TPooled>() where TPooled : IPoolable
        {
            Pool<TPooled> pool = FindPool<TPooled>();
            IPoolable spawned = pool.Get();
            return (TPooled) spawned;
        }

        public void Return<TPooled>(TPooled pooled) where TPooled : IPoolable
        {
            Pool<TPooled> pool = FindPool<TPooled>();
            pool.Return(pooled);
        }

        public void DestroyAllPools()
        {
            var keyValuePairs = _allPools.ToArray();
            
            for (var index = 0; index < keyValuePairs.Length; index++)
            {
                var pool = keyValuePairs[index];
                DestroyPool(pool.Key);
            }

            _allPools.Clear();
        }

        public void DestroyPool(Type poolKey)
        {
            if (_allPools.ContainsKey(poolKey))
            {
                _allPools.Remove(poolKey);
                return;
            }

#if DEBUG
            Debug.LogWarning("Pool is null!");
#endif
        }

        private Pool<TPooled> FindPool<TPooled>() where TPooled : IPoolable
        {
            IPool findPool = _allPools[typeof(TPooled)];
            return (Pool<TPooled>) findPool;
        }
    }

    internal interface IPoolKey
    {
    }
}
