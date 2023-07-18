using System;

namespace Services.Pools
{
    public interface IPoolService : IService
    {
        void InstallPool<TPooled>(Func<TPooled> preloadFunc, Action<TPooled> getAction,
            Action<TPooled> returnAction, int preloadCount) where TPooled : IPoolable;

        TPooled Get<TPooled>() where TPooled : IPoolable;
        void Return<TPooled>(TPooled pooled) where TPooled : IPoolable;
        void DestroyAllPools();
    }
}