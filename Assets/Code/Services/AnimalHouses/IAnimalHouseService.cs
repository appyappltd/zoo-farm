using System;
using System.Collections.Generic;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;

namespace Services.AnimalHouses
{
    public interface IAnimalHouseService : IService
    {
        void TakeQueueToHouse(AnimalId animalId, Func<IAnimal> callback);
        void RegisterHouse(AnimalHouse position);
        void VacateHouse(AnimalId withAnimalId);
        IReadOnlyList<AnimalId> AnimalsInQueue { get; }
    }
}