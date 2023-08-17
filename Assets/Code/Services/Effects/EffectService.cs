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

        public Effect SpawnEffect(EffectId id, Location location)
        {
            Effect effect = _poolService.Get<Effect>(new PoolKey(id));
            effect.Play(location);
            return effect;
        }

        public Effect SpawnEffect(EffectId id, Vector3 position, Quaternion rotation)
        {
            Effect effect = _poolService.Get<Effect>(new PoolKey(id));
            effect.Play(position, rotation);
            return effect;
        }
    }
}