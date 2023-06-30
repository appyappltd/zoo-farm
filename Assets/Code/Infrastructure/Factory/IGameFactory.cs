using System.Collections.Generic;
using Logic.Animals;
using Logic.Medicine;
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
        void Cleanup();
        void WarmUp();
        GameObject CreateHud();
        GameObject CreateHero(Vector3 vector3);
        GameObject CreateAnimal(AnimalType animalType, Vector3 at);
        GameObject CreateAnimalHouse(Vector3 at, Quaternion rotation);
        GameObject CreateBuildCell(Vector3 at, Quaternion rotation);
        GameObject CreateVisual(VisualType visual, Quaternion quaternion, Transform container);
        GameObject CreateCollectibleCoin(Transform container);
        GameObject CreateGardenBad(Vector3 at, Quaternion rotation);
        GameObject CreateMedBed(Vector3 at, Quaternion rotation);
        GameObject CreateMedTool(Vector3 at, Quaternion rotation, MedicineType toolType);
    }
}