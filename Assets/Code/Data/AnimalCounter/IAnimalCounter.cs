using System;
using System.Collections.Generic;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Services.Animals;

namespace Data.AnimalCounter
{
    public interface IAnimalCounter
    {
        event Action<AnimalType, AnimalCountData> Updated;
        void Register(IAnimal animal);
        void Unregister(IAnimal animal);

        AnimalCountData GetAnimalCountData(AnimalType byType);
        IReadOnlyDictionary<AnimalType, AnimalCountData> GetAllData();
        IReadOnlyCollection<AnimalType> GetAvailableAnimalTypes();
    }
}