using System;
using System.Collections.Generic;
using Logic.Animals.AnimalFeeders;
using Logic.Foods.FoodSettings;

namespace Services.Feeders
{
    public class AnimalFeederService : IAnimalFeederService
    {
        private readonly Dictionary<FoodId, AnimalFeeder> _feeders = new Dictionary<FoodId, AnimalFeeder>();

        public event Action Updated = () => { };
        
        public void RegisterFeeder(AnimalFeeder feeder)
        {
            if (_feeders.TryAdd(feeder.FoodId, feeder) == false)
                throw new ArgumentOutOfRangeException(nameof(feeder));
            
            Updated.Invoke();
        }

        public AnimalFeeder GetFeeder(FoodId byFoodId)
        {
            if (_feeders.TryGetValue(byFoodId, out AnimalFeeder feeder))
                return feeder;

            throw new ArgumentOutOfRangeException(nameof(feeder));
        }

        public bool HasFeeder(FoodId animalIdEdibleFood) =>
            _feeders.ContainsKey(animalIdEdibleFood);
    }
}