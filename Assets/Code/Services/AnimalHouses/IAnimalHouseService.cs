using System.Collections.Generic;
using Logic.Animals;

namespace Services.AnimalHouses
{
    public interface IAnimalHouseService : IService
    {
        void TakeQueueToHouse(QueueToHouse queueToHouse);
        void RegisterHouse(AnimalHouse position);
        void VacateHouse(AnimalId withAnimalId);
        IReadOnlyList<QueueToHouse> AnimalsInQueue { get; }
        bool TryGetNextFeedHouse(out AnimalHouse feedHouse);
    }
}