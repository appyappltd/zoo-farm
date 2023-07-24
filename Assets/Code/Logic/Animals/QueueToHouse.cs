using System;
using Logic.Animals.AnimalsBehaviour;

namespace Logic.Animals
{
    public struct QueueToHouse
    {
        public AnimalId AnimalId { get; }
        public Func<IAnimal> OnTakeHouse { get; }

        public QueueToHouse(AnimalId animalId, Func<IAnimal> onTakeHouse)
        {
            OnTakeHouse = onTakeHouse;
            AnimalId = animalId;
        }
    }
}