using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour.Emotions;
using Logic.Foods.FoodSettings;
using Logic.Medical;
using Logic.SpawnPlaces;
using Services.Effects;
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
        private const string WindowConfigPath = "StaticData/WindowConfigs";
        private const string MedStandConfigPath = "StaticData/MedStandConfigs";
        private const string SpawnPlaceConfigPath = "StaticData/SpawnPlaceConfig";
        private const string GardenBedConfigPath = "StaticData/GardenBedConfigs";
        private const string AnimalIconConfigPath = "StaticData/AnimalIconConfigs";
        private const string ParticleConfigPath = "StaticData/ParticleConfigs";
        
        private Dictionary<EmotionId, EmotionConfig> _emotionConfigs;
        private Dictionary<WindowId, WindowConfig> _windows;
        private Dictionary<MedicalToolId, MedToolStandConfig> _medStandConfigs;
        private Dictionary<FoodId, GardenBedConfig> _gardenBedConfigs;
        
        private SpawnPlaceConfig _spawnPlaceConfig;
        private AnimalIconConfig _animalIcons;
        private ParticleConfig _particlesConfig;

        public void Load()
        {
            _emotionConfigs = LoadFor<EmotionId, EmotionConfig>(EmotionConfigPath, x => x.Name);
            _medStandConfigs = LoadFor<MedicalToolId, MedToolStandConfig>(MedStandConfigPath, x => x.Type);
            _gardenBedConfigs = LoadFor<FoodId, GardenBedConfig>(GardenBedConfigPath, x => x.FoodId);

            _animalIcons = Resources.Load<AnimalIconConfig>(AnimalIconConfigPath);
            _spawnPlaceConfig = Resources.Load<SpawnPlaceConfig>(SpawnPlaceConfigPath);
            _particlesConfig = Resources.Load<ParticleConfig>(ParticleConfigPath);
            _windows = Resources
                .Load<WindowStaticData>(WindowConfigPath)
                .Configs
                .ToDictionary(x => x.WindowId, x => x);
        }

        public Emotion EmotionById(EmotionId emotionId)
        {
            EmotionConfig emotionConfig = GetDataFor(emotionId, _emotionConfigs);
            return new Emotion(emotionId, emotionConfig.Sprite);
        }

        public Transform SpawnPlaceById(SpawnPlaceId placeId) =>
            _spawnPlaceConfig.SpawnPlaces[placeId].SpawnPlaceByDefault;

        public Sprite IconByAnimalType(AnimalType animalIdType) =>
            _animalIcons.AnimalIcons[animalIdType];

        public ParticleSystem ParticlesById(EffectId id) =>
            _particlesConfig.Particles[id];

        public ParticleConfig ParticlesConfig() =>
            _particlesConfig;

        public MedToolStandConfig MedStandConfigById(MedicalToolId toolIdId) =>
            GetDataFor(toolIdId, _medStandConfigs);

        public GardenBedConfig GardenBedConfigById(FoodId foodId) =>
            GetDataFor(foodId, _gardenBedConfigs);

        public WindowBase WindowById(WindowId windowId) =>
            GetDataFor(windowId, _windows).Template;

        private TData GetDataFor<TKey, TData>(TKey key, IReadOnlyDictionary<TKey, TData> from) =>
            from.TryGetValue(key, out TData staticData)
                ? staticData
                : throw new NullReferenceException(
                    $"There is no {from.First().Value.GetType().Name} data with key: {key}");

        private Dictionary<TKey, TData> LoadFor<TKey, TData>(string path, Func<TData, TKey> keySelector)
            where TData : ScriptableObject =>
            Resources
                .LoadAll<TData>(path)
                .ToDictionary(keySelector, x => x);
    }
}