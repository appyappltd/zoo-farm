using System;
using System.Collections.Generic;
using Services.Effects;
using UnityEngine;

namespace Logic.Effects
{
    public class EffectSpawner : IDisposable
    {
        private readonly IEffectService _effectService;
        private readonly Dictionary<EffectId, Func<Effect>> _effectSpawnFunctions = new Dictionary<EffectId, Func<Effect>>();

        public EffectSpawner(IEffectService effectService)
        {
            _effectService = effectService;
        }

        public void InitEffect(EffectId effectId, Vector3 spawnAt, Quaternion spawnRotation, Transform parent = null)
        {
            var location = new Location(spawnAt, spawnRotation);

            if (_effectSpawnFunctions.ContainsKey(effectId))
            {
#if DEBUG
                Debug.LogWarning($"Effect spawner a;ready contains effect {effectId}");
#endif
                return;
            }
               

            _effectSpawnFunctions.Add(effectId, () => SpawnFunction(effectId, location, parent));
        }

        public Effect Spawn(EffectId effectId)
        {
            if (_effectSpawnFunctions.TryGetValue(effectId, out Func<Effect> spawnFunction))
                return spawnFunction.Invoke();

            throw new NullReferenceException(nameof(spawnFunction));
        }

        public void Dispose() =>
            _effectSpawnFunctions.Clear();

        private Effect SpawnFunction(EffectId effectId, Location location, Transform parent)
        {
            Effect effect = _effectService.SpawnEffect(effectId, location);
            effect.transform.SetParent(parent, true);
            return effect;
        }
    }
}