using System.Collections.Generic;
using Infrastructure.AssetManagement;
using Logic.Effects;
using Services.Effects;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class EffectFactory : IEffectFactory
    {
        private readonly IAssetProvider _assetProvider;

        private Dictionary<EffectId, GameObject> _cache;

        public EffectFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public Effect CreateParticle(EffectId id)
        {
            if (_cache.TryGetValue(id, out GameObject effect))
            {
                return _assetProvider.Instantiate(effect).GetComponent<Effect>();
            }
            
            GameObject newEffect = _assetProvider.Instantiate($"{AssetPath.EffectsPath}/{id}");
            _cache.Add(id, newEffect);

            return newEffect.GetComponent<Effect>();
        }
    }
}