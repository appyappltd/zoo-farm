using System;
using System.Collections.Generic;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using UnityEngine;

namespace Services.Breeding
{
    public interface IAnimalBreedService : IService
    {
        void Register(IAnimal animal);
        void Unregister(IAnimal animal);
        ICollection<AnimalType> GetAvailablePairTypes();
        bool TryBreeding(AnimalType breedingAnimalType, out AnimalPair pair);
        void BeginBreeding(AnimalPair pair, Transform at, Action onBeginsCallback = null);
    }
}