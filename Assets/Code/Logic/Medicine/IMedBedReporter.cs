using System;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;

namespace Logic.Medicine
{
    public interface IMedBedReporter
    {
        event Action<AnimalId> Healed;
        event Action<AnimalId> HouseFound;
    }
}