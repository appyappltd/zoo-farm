using System;
using System.Collections.Generic;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;

namespace Services.Animals
{
    public interface IAnimalsService : IService
    {
        event Action<IAnimal> Released;
        int TotalAnimalCount { get; }
        int ReleaseReadyAnimalCount { get; }
        IReadOnlyList<IAnimal> Animals { get; }
        void Register(IAnimal animal);
        void Release(IAnimal animal);
        void Release(AnimalType animalType);
        IAnimal[] GetAnimals(AnimalType panelAnimalType);
        AnimalCountData GetAnimalsCount(AnimalType panelAnimalType);
    }
}