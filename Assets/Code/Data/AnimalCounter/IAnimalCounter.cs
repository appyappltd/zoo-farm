using System;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Services.Animals;

namespace Data
{
    public interface IAnimalCounter
    {
        event Action<AnimalType, AnimalCountData> Updated;
        void Register(IAnimal animal);
        void Unregister(IAnimal animal);

        AnimalCountData GetAnimalCountData(AnimalType byType);
    }
}