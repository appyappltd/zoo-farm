using Logic.Plants.PlantSettings;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IPlantFactory
    {
        GameObject CreatePlant(Vector3 spawnPlacePosition, Quaternion spawnPlaceRotation, PlantId configPlantId, int stageId);
    }
}