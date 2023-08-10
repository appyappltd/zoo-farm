using System.Collections.Generic;
using Logic.Animals;

namespace Services.AnimalHouses
{
    public interface IAnimalHouseService : IService
    {
        void TakeQueueToHouse(QueueToHouse queueToHouse, bool isHighPriority = false);
        void RegisterHouse(AnimalHouse position);
        void VacateHouse(AnimalId withAnimalId);
        IReadOnlyList<QueueToHouse> AnimalsInQueue { get; }
        bool TryGetNextFeedHouse(out AnimalHouse feedHouse);
    }
}