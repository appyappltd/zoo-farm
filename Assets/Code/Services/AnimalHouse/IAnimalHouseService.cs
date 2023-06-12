using System;
using Logic.AnimalsBehaviour;

namespace Services.AnimalHouse
{
    public interface IAnimalHouseService : IService
    {
        void TakeQueueToHouse(Func<Animal> callback);
        void BuildHouse();
    }
}