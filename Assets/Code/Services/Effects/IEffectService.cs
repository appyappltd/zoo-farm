using Logic;
using Logic.Effects;
using UnityEngine;

namespace Services.Effects
{
    public interface IEffectService : IService
    {
        Effect SpawnEffect(EffectId id, Vector3 position, Quaternion rotation);
        Effect SpawnEffect(EffectId id, Location location);
    }
}   