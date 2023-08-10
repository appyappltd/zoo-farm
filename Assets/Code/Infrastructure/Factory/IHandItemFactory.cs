using Logic.Animals;
using Data.ItemsData;
using Logic.Foods.FoodSettings;
using Logic.Medical;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IHandItemFactory
    {
        HandItem CreateAnimal(Vector3 at, Quaternion rotation, AnimalType animalType);
        HandItem CreateFood(Vector3 at, Quaternion rotation, FoodId foodId);
        HandItem CreateMedicalToolItem(Vector3 at, Quaternion rotation, MedicalToolId toolId);
    }
}