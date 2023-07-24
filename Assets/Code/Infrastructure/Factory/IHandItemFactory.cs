using Data.ItemsData;
using Logic.Animals;
using Logic.Foods.FoodSettings;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IHandItemFactory
    {
        HandItem CreateAnimal(Vector3 at, Quaternion rotation, AnimalType animalType);
        HandItem CreateFood(Vector3 at, Quaternion rotation, FoodId foodId);
    }
}