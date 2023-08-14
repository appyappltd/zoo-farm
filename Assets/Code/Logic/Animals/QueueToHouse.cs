using System;
using Logic.Animals.AnimalsBehaviour;

namespace Logic.Animals
{
    public struct QueueToHouse
    {
        public IAnimal Animal { get; }
        public Func<IAnimal> OnTakeHouse { get; }

        public QueueToHouse(IAnimal animal, Func<IAnimal> onTakeHouse)
        {
            OnTakeHouse = onTakeHouse;
            Animal = animal;
        }
    }
}