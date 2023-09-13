using System;
using System.Collections.Generic;
using Logic.Animals;
using Services.Animals;

namespace Data.AnimalCounter
{
    public interface IAnimalCounter
    {
        event Action<AnimalType, AnimalCountData> Updated;
        int TotalBreedingReadyPairsCount { get; }

        AnimalCountData GetAnimalCountData(AnimalType byType);
        IReadOnlyDictionary<AnimalType, AnimalCountData> GetAllData();
        IReadOnlyCollection<AnimalType> GetAvailableAnimalTypes();
    }
}