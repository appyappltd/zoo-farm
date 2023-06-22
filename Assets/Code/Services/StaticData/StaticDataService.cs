using System;
using System.Collections.Generic;
using System.Linq;
using Logic.AnimalsBehaviour.Emotions;
using StaticData;
using UnityEngine;

namespace Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string EmotionConfigPath = "StaticData/EmotionConfigs";
        
        private Dictionary<EmotionId, EmotionConfig> _emotionConfigs;

        public void Load()
        {
            _emotionConfigs = LoadFor<EmotionConfig, EmotionId>(EmotionConfigPath, x => x.Name);
        }

        public Emotion EmotionById(EmotionId emotionId)
        {
            EmotionConfig emotionConfig = GetDataFor(emotionId, _emotionConfigs);
            return new Emotion(emotionId, emotionConfig.Sprite);
        }

        private TData GetDataFor<TData, TKey>(TKey key, IReadOnlyDictionary<TKey, TData> from) =>
            from.TryGetValue(key, out TData staticData)
                ? staticData
                : throw new NullReferenceException(
                    $"There is no {from.First().Value.GetType().Name} data with key: {key}");

        private Dictionary<TKey, TData> LoadFor<TData, TKey>(string path, Func<TData, TKey> keySelector)
            where TData : ScriptableObject =>
            Resources
                .LoadAll<TData>(path)
                .ToDictionary(keySelector, x => x);
    }
}