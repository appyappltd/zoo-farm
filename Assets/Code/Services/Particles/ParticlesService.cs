using AYellowpaper.SerializedCollections;
using Infrastructure.Factory;
using Logic;
using Services.Pools;
using Services.StaticData;
using UnityEngine;

namespace Services.Particles
{
    public class ParticlesService
    {
        private readonly IPoolService _poolService;
        private readonly IEffectFactory _effectFactory;

        private Transform _parent;

        public ParticlesService(IPoolService poolService, IStaticDataService staticDataService, IEffectFactory effectFactory)
        {
            _poolService = poolService;
            _effectFactory = effectFactory;
            _parent = new GameObject("Pool Particles Effects").transform;
            InitPools(staticDataService.ParticlesConfig().Particles);
        }

        private void InitPools(SerializedDictionary<ParticleId, ParticleSystem> particles)
        {
            foreach (var pair in particles)
            {
                _poolService.InstallPool(_effectFactory.CreateParticle(pair.Key), , );
            }
        }

        public void SpawnParticle(ParticleId id, Location location)
        {
            if(_poolService.G)
        }
    }
}