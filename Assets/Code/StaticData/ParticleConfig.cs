using AYellowpaper.SerializedCollections;
using Services.Particles;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "Static Data/Particle Configs", fileName = "ParticleConfigs", order = 0)]
    public class ParticleConfig : ScriptableObject
    {
        public SerializedDictionary<ParticleId, ParticleSystem> Particles;
    }
}