using AYellowpaper.SerializedCollections;
using Infrastructure.Factory;
using Logic;
using Logic.Effects;
using Services.Pools;
using Services.StaticData;
using UnityEngine;

namespace Services.Effects
{
    public class EffectService : IEffectService
    {
        private readonly IPoolService _poolService;
        private readonly IEffectFactory _effectFactory;
        private readonly Transform _parent;

        private const int DefaultPreloadCount = 2;

        public EffectService(IPoolService poolService, IStaticDataService staticDataService,
            IEffectFactory effectFactory)
        {
            _poolService = poolService;
            _effectFactory = effectFactory;
            _parent = new GameObject("Pool Particles Effects").transform;
            InitPools(staticDataService.ParticlesConfig().Particles);
        }

        public void InitPools(SerializedDictionary<EffectId, ParticleSystem> particles,
            int preloadCount = DefaultPreloadCount)
        {
            foreach (var (key, _) in particles)
            {
                _poolService.InstallPool(new PoolKey(key), () => _effectFactory.CreateParticle(key),
                    GetAction,
                    ReturnAction,
                    preloadCount);
            }
        }

        public void SpawnEffect(EffectId id, Location location)
        {
            Effect effect = _poolService.Get<Effect>(new PoolKey(id));
            effect.Play(location);
        }

        private void ReturnAction(Effect effect)
        {
            effect.GameObject.SetActive(false);
            effect.GameObject.transform.SetParent(_parent);
        }

        private void GetAction(Effect effect)
        {
            effect.GameObject.transform.SetParent(null);
            effect.GameObject.SetActive(true);
        }
    }
}