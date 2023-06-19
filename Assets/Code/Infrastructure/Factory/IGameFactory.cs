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
        GameObject CreateBuildCell(Vector3 at);
        GameObject CreateVisual(VisualType visual, Quaternion quaternion, Transform container);
        GameObject CreateCollectibleCoin(Transform container);
        GameObject CreateGardenBad(Vector3 at);
    }
}