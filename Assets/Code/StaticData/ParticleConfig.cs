using AYellowpaper.SerializedCollections;
using Services.Effects;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "Static Data/Particle Configs", fileName = "ParticleConfigs", order = 0)]
    public class ParticleConfig : ScriptableObject
    {
        public SerializedDictionary<EffectId, ParticleSystem> Particles;
    }
}