using Services.Effects;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IEffectFactory
    {
        ParticleSystem CreateParticle(EffectId id);
    }
}