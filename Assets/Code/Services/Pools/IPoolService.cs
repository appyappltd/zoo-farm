using System;

namespace Services.Pools
{
    public interface IPoolService : IService
    {
        void InstallPool<TPooled>(PoolKey key, Func<TPooled> preloadFunc, Action<TPooled> getAction,
            Action<TPooled> returnAction, int preloadCount) where TPooled : IPoolable;

        TPooled Get<TPooled>(PoolKey key) where TPooled : IPoolable;
        void Return<TPooled>(TPooled pooled, PoolKey key) where TPooled : IPoolable;
        void DestroyAllPools();
    }
}