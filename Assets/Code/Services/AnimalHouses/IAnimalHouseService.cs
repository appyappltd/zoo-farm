using System.Collections.Generic;
using Logic.Animals;

namespace Services.AnimalHouses
{
    public interface IAnimalHouseService : IService
    {
        IReadOnlyList<QueueToHouse> AnimalsInQueue { get; }
        void TakeQueueToHouse(QueueToHouse queueToHouse, bool isHighPriority = false);
        void RegisterHouse(IAnimalHouse position);
        void VacateHouse(AnimalId withAnimalId);
        bool TryGetNextFeedHouse(out IAnimalHouse feedHouse);
        bool TryGetNextAnimalInQueue(out AnimalId animalId);
    }
}