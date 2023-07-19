using System;
using Logic.Animals;

namespace Logic.Medical
{
    public interface IMedicalBedReporter
    {
        event Action<AnimalId> Healed;
        event Action<AnimalId> HouseFound;
    }
}