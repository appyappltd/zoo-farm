using System.Collections.Generic;
using Infrastructure.AssetManagement;
using Services.Particles;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class EffectFactory : IEffectFactory
    {
        private readonly IAssetProvider _assetProvider;

        private Dictionary<ParticleId, GameObject> _cache;

        public EffectFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public ParticleSystem CreateParticle(ParticleId id)
        {
            if (_cache.TryGetValue(id, out GameObject effect))
            {
                return _assetProvider.Instantiate(effect).GetComponent<ParticleSystem>();
            }
            
            GameObject newEffect = _assetProvider.Instantiate($"{AssetPath.EffectsPath}/{id}");
            _cache.Add(id, newEffect);

            return newEffect.GetComponent<ParticleSystem>();
        }
    }
}