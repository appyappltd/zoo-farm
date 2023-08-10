using Data.ItemsData;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour.Emotions;
using Logic.Foods.FoodSettings;
using Logic.Medical;
using Logic.SpawnPlaces;
using Services.Effects;
using StaticData;
using StaticData.ScaleModifiers;
using Ui.Services;
using Ui.Windows;
using UnityEngine;

namespace Services.StaticData
{
    public interface IStaticDataService : IService
    {
        void Load();
        Emotion EmotionById(EmotionId emotionId);
        WindowBase WindowById(WindowId windowId);
        Transform SpawnPlaceById(SpawnPlaceId placeId);
        MedToolStandConfig MedStandConfigById(MedicalToolId toolIdId);
        GardenBedConfig GardenBedConfigById(FoodId foodId);
        Sprite IconByAnimalType(AnimalType animalIdType);
        ParticleSystem ParticlesById(EffectId id);
        ParticleConfig ParticlesConfig();
        ScaleModifierConfig ScaleModifierById(ScaleModifierId id);
        AnimalItemStaticData AnimalItemDataById(AnimalType id);
    }
}