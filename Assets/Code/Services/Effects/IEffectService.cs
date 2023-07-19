using AYellowpaper.SerializedCollections;
using Logic;
using UnityEngine;

namespace Services.Effects
{
    public interface IEffectService : IService
    {
        void InitPools(SerializedDictionary<EffectId, ParticleSystem> particles,
            int preloadCount = default);

        void SpawnEffect(EffectId id, Location location);
    }
}