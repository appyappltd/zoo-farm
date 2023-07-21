using Data.ItemsData;
using Logic.Animals;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IHandItemFactory
    {
        HandItem CreateAnimal(Vector3 at, Quaternion rotation, AnimalType animalType);
        HandItem CreateFood(Vector3 at, Quaternion rotation);
    }
}