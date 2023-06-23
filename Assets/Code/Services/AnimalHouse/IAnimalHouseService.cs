using System;
using Logic.AnimalsBehaviour;
using UnityEngine;

namespace Services.AnimalHouse
{
    public interface IAnimalHouseService : IService
    {
        void TakeQueueToHouse(Func<IAnimal> callback);
        void BuildHouse(Vector3 position);
    }
}