using System;
using Logic.Animals;

namespace Data
{
    [Serializable]
    public class MedBedData
    {
        public byte Id;
        public AnimalType WaitingForHealAnimal;
        public AnimalId WaitingForHouseAnimal;
    }
}