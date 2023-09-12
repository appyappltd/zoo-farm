using Data;
using Logic.Animals;
using Data.ItemsData;
using Logic.Foods.FoodSettings;
using Logic.Medical;
using Logic.Storages.Items;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IHandItemFactory
    {
        HandItem CreateAnimal(Vector3 at, Quaternion rotation, AnimalType animalType,
            AnimalAndTreatToolPair pair);
        HandItem CreateFood(Vector3 at, Quaternion rotation, FoodId foodId);
        HandItem CreateMedicalToolItem(Vector3 at, Quaternion rotation, TreatToolId toolId);
        HandItem CreateBreedingCurrency(Vector3 zero, Quaternion identity);
    }
}