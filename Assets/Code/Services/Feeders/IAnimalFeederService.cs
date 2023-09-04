using System;
using Logic.Animals.AnimalFeeders;
using Logic.Foods.FoodSettings;
using UnityEngine;

namespace Services.Feeders
{
    public interface IAnimalFeederService : IService
    {
        void RegisterFeeder(AnimalFeeder feeder);
        AnimalFeeder GetFeeder(FoodId byFoodId);
        bool HasFeeder(FoodId animalIdEdibleFood);
        event Action Updated;
    }
}