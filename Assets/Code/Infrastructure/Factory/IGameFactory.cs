using System.Collections.Generic;
using Logic.Interactions;
using Logic.Animals;
using Data.ItemsData;
using Logic.Animals.AnimalsBehaviour;
using Logic.Foods.FoodSettings;
using Logic.Medical;
using Logic.Payment;
using Logic.Spawners;
using Services;
using Services.PersistentProgress;
using Ui;
using Ui.LevelGoalPanel;
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
        GameObject CreateBuildCell(Vector3 at, Quaternion rotation, ConsumeType consumeType);
        GameObject CreateVisual(VisualType visual, Quaternion quaternion);
        GameObject CreateCollectibleCoin();
        GameObject CreateFoodVendor(Vector3 at, Quaternion rotation, FoodId foodId);
        GameObject CreateVolunteer(Vector3 at, Transform parent);
        GameObject CreateKeeper(Vector3 markerBuildPosition);
        GameObject CreateAnimalChild(Vector3 at, Quaternion rotation, AnimalType type);
        MedicalBed CreateMedBed(Vector3 at, Quaternion rotation);
        MedicalToolStand CreateMedToolStand(Vector3 at, Quaternion rotation, MedicalToolId toolIdType);
        Animal CreateAnimal(AnimalItemStaticData animalData, Vector3 at, Quaternion rotation);
        Animal CreateAnimal(IAnimal clone, Vector3 at, Quaternion rotation);
        GameObject CreateHouseFoundation(Vector3 markerBuildPosition, Quaternion locationRotation);
        GameObject CreateBreedingPlace(Vector3 at, Quaternion rotation);
        ReleaseInteractionProvider CreateReleaseInteraction(Vector3 at, Quaternion rotation, AnimalType withType);
        IAnimalHouse CreateAnimalHouse(Vector3 at, Quaternion rotation, AnimalType animalType);
        ChoseInteractionProvider CreateChoseInteraction(Vector3 at, Quaternion rotation, AnimalType withType);
        ChoseInteractionProvider CreateChoseInteraction(Vector3 at, Quaternion rotation, FoodId withType);
        GoalAnimalPanelProvider CreateAnimalGoalPanel(Vector3 at, Quaternion rotation, Transform parent, KeyValuePair<AnimalType, int> countTypePair);
        GameObject CreateFeederFoundation(Vector3 at, Quaternion rotation);
        GameObject CreateFeeder(Vector3 at, Quaternion rotation, FoodId withFoodId);
    }
}