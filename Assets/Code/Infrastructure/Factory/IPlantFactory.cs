using Logic.Foods.FoodSettings;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IFoodFactory
    {
        GameObject CreateFood(Vector3 spawnPlacePosition, Quaternion spawnPlaceRotation, FoodId configFoodId, int stageId);
    }
}