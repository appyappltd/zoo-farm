using System;
using Logic.Animals.AnimalsBehaviour;

namespace Logic.Animals
{
    public struct QueueToHouse
    {
        public IAnimal Animal { get; }
        public Action OnTakeHouse { get; }

        public QueueToHouse(IAnimal animal, Action onTakeHouse)
        {
            OnTakeHouse = onTakeHouse;
            Animal = animal;
        }
    }
}