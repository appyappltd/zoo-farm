using System;
using System.Collections.Generic;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;

namespace Services.AnimalHouses
{
    public interface IAnimalHouseService : IService
    {
        void TakeQueueToHouse(QueueToHouse queueToHouse);
        void RegisterHouse(AnimalHouse position);
        void VacateHouse(AnimalId withAnimalId);
        IReadOnlyList<QueueToHouse> AnimalsInQueue { get; }
    }
}