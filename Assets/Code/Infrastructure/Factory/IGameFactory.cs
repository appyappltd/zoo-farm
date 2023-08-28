using System.Collections.Generic;
using Logic.Animals;
using Data.ItemsData;
using Logic.Animals.AnimalsBehaviour;
using Logic.Foods.FoodSettings;
using Logic.LevelGoals;
using Logic.Medical;
using Logic.Spawners;
using Services;
using Services.PersistentProgress;
using Ui;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        IFoodFactory FoodFactory { get; }
        IEffectFactory EffectFactory { get; }
        IHandItemFactory HandItemFactory { get; }
        void Cleanup();
        void WarmUp();
        GameObject CreateHud();
        GameObject CreateHero(Vector3 vector3);
        Animal CreateAnimal(AnimalItemStaticData animalData, Vector3 at, Quaternion rotation);
        Animal CreateAnimal(IAnimal clone, Vector3 at, Quaternion rotation);
        IAnimalHouse CreateAnimalHouse(Vector3 at, Quaternion rotation, AnimalType animalType);
        GameObject CreateBuildCell(Vector3 at, Quaternion rotation);
        GameObject CreateVisual(VisualType visual, Quaternion quaternion);
        GameObject CreateCollectibleCoin();
        GameObject CreateFoodVendor(Vector3 at, Quaternion rotation, FoodId foodId);
        MedicalBed CreateMedBed(Vector3 at, Quaternion rotation);
        MedicalToolStand CreateMedToolStand(Vector3 at, Quaternion rotation, MedicalToolId toolIdType);
        GameObject CreateVolunteer(Vector3 at, Transform parent);
        GameObject CreateHandItem(Vector3 at, Quaternion rotation, ItemId itemId);
        GameObject CreateKeeper(Vector3 markerBuildPosition);
        GameObject CreateAnimalChild(Vector3 at, Quaternion rotation, AnimalType type);
        IAnimalHouse CreateBreedingHouse(Vector3 at, Quaternion rotation);
        ReleaseInteractionProvider CreateReleaseInteraction(Vector3 at, Quaternion rotation, AnimalType withType);
        GoalAnimalPanelProvider CreateAnimalGoalPanel(Vector3 at, Quaternion rotation, Transform parent, KeyValuePair<AnimalType, int> countTypePair);
    }
}