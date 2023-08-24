using System.Collections.Generic;
using Infrastructure.AssetManagement;
using Logic.Effects;
using Services.Effects;
using Services.Pools;
using Services.StaticData;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class EffectFactory : IEffectFactory
    {
        private const int DefaultPreloadCount = 5;
        private const int CacheCapacity = 32;
        
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly IPoolService _poolService;
        private readonly Dictionary<EffectId, GameObject> _cache;

        private Transform _parent;

        public EffectFactory(IAssetProvider assetProvider, IStaticDataService staticDataService, IPoolService poolService)
        {
            _cache = new Dictionary<EffectId, GameObject>(CacheCapacity);
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _poolService = poolService;
        }

        public void WarmUp()
        {
            _parent = new GameObject("Pool Particles Effects").transform;

            foreach (var (key, _) in _staticDataService.ParticlesConfig().Particles)
            {
                _poolService.InstallPool(new PoolKey(key), () => CreateParticle(key),
                    GetAction,
                    ReturnAction,
                    DefaultPreloadCount,
                    _parent);
            }
        }
        
        public Effect CreateParticle(EffectId id)
        {
            if (_cache.TryGetValue(id, out GameObject effect))
            {
                return _assetProvider.Instantiate(effect).GetComponent<Effect>();
            }
            
            GameObject newEffect = _assetProvider.Instantiate($"{AssetPath.EffectsPath}/{id}");
            _cache.Add(id, newEffect);

            return newEffect.GetComponent<Effect>();
        }

        public void Cleanup()
        {
            _cache.Clear();
        }

        private void ReturnAction(Effect effect)
        {
            effect.GameObject.SetActive(false);
        }

        private void GetAction(Effect effect)
        {
            effect.GameObject.SetActive(true);
        }
    }
}