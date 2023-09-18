using System;
using System.Collections.Generic;
using Logic.Animals;
using UnityEngine;

namespace Services.Breeding
{
    public interface IAnimalBreedService : IService
    {
        ICollection<AnimalType> GetAvailablePairTypes();
        bool TryBreeding(AnimalType breedingAnimalType, out AnimalPair pair);
        void BeginBreeding(AnimalPair pair, Transform at, Action onBeginsCallback = null, Action onCompleteCallback = null);
    }
}