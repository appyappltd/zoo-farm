using System.Collections.Generic;
using Data.ItemsData;
using Logic.Animals;
using Logic.Foods.FoodSettings;
using Logic.Medical;
using Logic.Spawners;
using Services;
using Services.PersistentProgress;
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
        GameObject CreateAnimal(AnimalItemData animalData, Vector3 at, Quaternion rotation);
        GameObject CreateAnimalHouse(Vector3 at, Quaternion rotation, AnimalType animalType);
        GameObject CreateBuildCell(Vector3 at, Quaternion rotation);
        GameObject CreateVisual(VisualType visual, Quaternion quaternion);
        GameObject CreateCollectibleCoin();
        GameObject CreateFoodVendor(Vector3 at, Quaternion rotation, FoodId foodId);
        GameObject CreateMedBed(Vector3 at, Quaternion rotation);
        GameObject CreateMedToolStand(Vector3 at, Quaternion rotation, MedicalToolId toolIdType);
        GameObject CreateVolunteer(Vector3 at, Transform parent);
        GameObject CreateHandItem(Vector3 at, Quaternion rotation, ItemId itemId);
        GameObject CreateKeeper(Vector3 markerBuildPosition);
    }
}