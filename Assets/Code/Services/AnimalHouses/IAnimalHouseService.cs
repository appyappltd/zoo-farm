using System;
using Logic.Animals.AnimalsBehaviour;

namespace Services.AnimalHouses
{
    public interface IAnimalHouseService : IService
    {
        void TakeQueueToHouse(Func<IAnimal> callback);
        void RegisterHouse(Logic.Animals.AnimalHouse position);
        void VacateHouse(AnimalId withAnimalId);
    }
}