using Services.Particles;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IEffectFactory
    {
        ParticleSystem CreateParticle(ParticleId id);
    }
}