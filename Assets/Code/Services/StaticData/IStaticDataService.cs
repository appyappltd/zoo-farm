using Logic.Animals.AnimalsBehaviour.Emotions;
using Logic.Medicine;
using Logic.SpawnPlaces;
using StaticData;
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
        MedToolStandConfig MedStandConfigById(MedicineToolId toolIdId);
    }
}