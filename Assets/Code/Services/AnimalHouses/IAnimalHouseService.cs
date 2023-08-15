using System.Collections.Generic;
using Logic.Animals;

namespace Services.AnimalHouses
{
    public interface IAnimalHouseService : IService
    {
        IReadOnlyList<QueueToHouse> AnimalsInQueue { get; }
        int AnimalsInHouseQueueCount { get; }
        void TakeQueueToHouse(QueueToHouse queueToHouse, bool isHighPriority = false);
        void RegisterHouse(IAnimalHouse position);
        void VacateHouse(AnimalId withAnimalId);
        bool TryGetNextFeedHouse(out IAnimalHouse feedHouse);
        bool TryGetNextAnimalIdInQueue(out AnimalId animalId);
        IEnumerable<AnimalType> GetAnimalTypesInHouseQueue(bool isHighPriority = false);
    }
}