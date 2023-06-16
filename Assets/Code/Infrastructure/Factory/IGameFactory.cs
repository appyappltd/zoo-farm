using System.Collections.Generic;
using Logic.Spawners;
using Pool;
using Services;
using Services.PersistentProgress;
using Services.SaveLoad;
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
        GameObject CreateAnimalHouse(Vector3 at);
        GameObject CreateHouseCell(Vector3 peek);
        GameObject CreateVisual(VisualType visual, Quaternion identity, Transform container);
        GameObject CreateCollectibleCoin(Transform container);
    }
}