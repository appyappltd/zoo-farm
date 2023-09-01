using System;
using System.Collections.Generic;
using Data;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;

namespace Services.Animals
{
    public interface IAnimalsService : IService
    {
        event Action<IAnimal> Released;
        event Action<IAnimal> Registered;
        int TotalAnimalCount { get; }
        int ReleaseReadyAnimalCount { get; }
        IReadOnlyList<IAnimal> Animals { get; }
        IAnimalCounter AnimalCounter { get; }
        IEnumerable<AnimalType> GetBreedingReady();
        void Register(IAnimal animal);
        void Release(IAnimal animal);
        void Release(AnimalType animalType);
        BreedingPair SelectPairForBreeding(AnimalType byType);
        IEnumerable<IAnimal> GetReleaseReady();
        IAnimal GetReleaseReadySingle(AnimalType withType);
    }
}