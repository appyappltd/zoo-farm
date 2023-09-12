using System;
using System.Collections.Generic;
using System.Linq;
using Data.ItemsData;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour.Emotions;
using Logic.Foods.FoodSettings;
using Logic.LevelGoals;
using Logic.Medical;
using Logic.SpawnPlaces;
using Services.Effects;
using StaticData;
using StaticData.ScaleModifiers;
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
        private const string IconsConfigPath = "StaticData/IconsConfig";
        private const string ParticleConfigPath = "StaticData/ParticleConfigs";
        private const string ScaleModifierPath = "StaticData/ScaleModifierConfigs";
        private const string AnimalItemsPath = "StaticData/HandItemConfigs/Animals";
        private const string LevelGoalConfigPath = "StaticData/LevelGoalConfigs";
        private const string FoodConfigPath = "StaticData/HandItemConfigs/Food";
        private const string MedicalConfigPath = "StaticData/HandItemConfigs/MedicalTools";
        private const string DefaultItemConfigPath = "StaticData/HandItemConfigs";

        private Dictionary<EmotionId, EmotionConfig> _emotionConfigs;
        private Dictionary<WindowId, WindowConfig> _windows;
        private Dictionary<TreatToolId, MedToolStandConfig> _medStandConfigs;
        private Dictionary<ScaleModifierId, ScaleModifierConfig> _scaleModifierConfigs;
        private Dictionary<AnimalType, AnimalItemStaticData> _animalItemConfigs;
        private Dictionary<string, GoalConfig> _levelGoalConfigs;
        private Dictionary<FoodId, FoodItemData> _foodConfigs;
        private Dictionary<TreatToolId, MedicalToolItemData> _medicalToolConfigs;
        private Dictionary<ItemId, DefaultItemData> _defaultItemConfigs;

        private SpawnPlaceConfig _spawnPlaceConfig;
        private IconConfig _icons;
        private ParticleConfig _particlesConfig;

        public void Load()
        {
            _emotionConfigs = LoadFor<EmotionId, EmotionConfig>(EmotionConfigPath, x => x.Name);
            _medStandConfigs = LoadFor<TreatToolId, MedToolStandConfig>(MedStandConfigPath, x => x.Type);
            _scaleModifierConfigs = LoadFor<ScaleModifierId, ScaleModifierConfig>(ScaleModifierPath, x => x.ModifierId);
            _animalItemConfigs = LoadFor<AnimalType, AnimalItemStaticData>(AnimalItemsPath, x => x.AnimalType);
            _levelGoalConfigs = LoadFor<string, GoalConfig>(LevelGoalConfigPath, x => x.LevelName);
            _foodConfigs = LoadFor<FoodId, FoodItemData>(FoodConfigPath, x => x.FoodId);
            _medicalToolConfigs = LoadFor<TreatToolId, MedicalToolItemData>(MedicalConfigPath, x => x.MedicineToolId);
            _defaultItemConfigs = LoadFor<ItemId, DefaultItemData>(DefaultItemConfigPath, x => x.ItemId);

            _icons = Resources.Load<IconConfig>(IconsConfigPath);
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
            _icons.AnimalIcons[animalIdType];

        public Sprite IconByFoodType(FoodId foodId) =>
            _icons.FoodIcons[foodId];

        public Sprite IconByTreatToolType(TreatToolId treatToolId) =>
            _icons.TreatToolsIcons[treatToolId];

        public GoalConfig GoalConfigForLevel(string levelName) =>
            GetDataFor(levelName, _levelGoalConfigs);

        public FoodItemData FoodItemDataById(FoodId foodId) =>
            GetDataFor(foodId, _foodConfigs);

        public MedicalToolItemData MedicalItemDataById(TreatToolId medicalToolId) =>
            GetDataFor(medicalToolId, _medicalToolConfigs);

        public DefaultItemData DefaultItemDataById(ItemId itemId) =>
            GetDataFor(itemId, _defaultItemConfigs);

        public ScaleModifierConfig ScaleModifierById(ScaleModifierId id) =>
            GetDataFor(id, _scaleModifierConfigs);

        public AnimalItemStaticData AnimalItemDataById(AnimalType id) =>
            GetDataFor(id, _animalItemConfigs);
        
        public ParticleSystem ParticlesById(EffectId id) =>
            _particlesConfig.Particles[id];

        public ParticleConfig ParticlesConfig() =>
            _particlesConfig;

        public MedToolStandConfig MedStandConfigById(TreatToolId toolIdId) =>
            GetDataFor(toolIdId, _medStandConfigs);

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