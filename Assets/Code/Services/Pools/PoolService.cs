using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Services.Pools
{
    public class PoolService : IPoolService
    {
        private readonly Dictionary<PoolKey, IPool> _allPools = new Dictionary<PoolKey, IPool>(16);

        public void InstallPool<TPooled>(PoolKey key, Func<TPooled> preloadFunc, Action<TPooled> getAction,
            Action<TPooled> returnAction, int preloadCount, Transform parent = null) where TPooled : IPoolable
        {
#if DEBUG
            Debug.Log($"Pool key {typeof(TPooled).Name} hashCode {key.ToString()} ");
#endif
            
            if (_allPools.ContainsKey(key))
            {
#if DEBUG
                Debug.LogWarning($"Pool with type {nameof(TPooled)} already exists");
#endif
                return;
            }

            Pool<TPooled> newPool = new Pool<TPooled>(preloadFunc, getAction, returnAction, preloadCount, parent);
            _allPools.Add(key, newPool);
        }

        public TPooled Get<TPooled>(PoolKey key) where TPooled : IPoolable
        {
            Pool<TPooled> pool = FindPool<TPooled>(key);
            IPoolable spawned = pool.Get();
            return (TPooled) spawned;
        }

        public void Return<TPooled>(TPooled pooled, PoolKey key) where TPooled : IPoolable
        {
            Pool<TPooled> pool = FindPool<TPooled>(key);
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

        public void DestroyPool(PoolKey poolKey)
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

        private Pool<TPooled> FindPool<TPooled>(PoolKey key) where TPooled : IPoolable
        {
            IPool found = _allPools[key];

            if (found is Pool<TPooled> pool)
            {
                return pool;
            }

            throw new InvalidCastException(nameof(pool));
        }
    }
}
