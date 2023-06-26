using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Animals.AnimalsBehaviour.Emotions;
using StaticData;
using StaticData.Windows;
using Ui.Services;
using Ui.Windows;
using UnityEngine;

namespace Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string EmotionConfigPath = "StaticData/EmotionConfigs";
        private const string WindowPath = "StaticData/WindowConfig/WindowConfigs";
        
        private Dictionary<EmotionId, EmotionConfig> _emotionConfigs;
        private Dictionary<WindowId, WindowConfig> _windows;

        public void Load()
        {
            _emotionConfigs = LoadFor<EmotionConfig, EmotionId>(EmotionConfigPath, x => x.Name);
            _windows = Resources
                .Load<WindowStaticData>(WindowPath)
                .Configs
                .ToDictionary(x => x.WindowId, x => x);
        }

        public Emotion EmotionById(EmotionId emotionId)
        {
            EmotionConfig emotionConfig = GetDataFor(emotionId, _emotionConfigs);
            return new Emotion(emotionId, emotionConfig.Sprite);
        }
        
        public WindowBase WindowById(WindowId windowId) =>
            GetDataFor(windowId, _windows).Template;

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