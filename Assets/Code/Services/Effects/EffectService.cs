using Logic;
using Logic.Effects;
using Services.Pools;
using UnityEngine;

namespace Services.Effects
{
    public class EffectService : IEffectService
    {
        private readonly IPoolService _poolService;

        private Transform _parent;

        public EffectService(IPoolService poolService)
        {
            _poolService = poolService;
        }

        public void SpawnEffect(EffectId id, Location location)
        {
            Effect effect = _poolService.Get<Effect>(new PoolKey(id));
            effect.Play(location);
        }
    }
}