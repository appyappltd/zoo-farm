using Data;
using Logic.TransformGrid;
using Services.StaticData;

namespace Logic.Animals
{
    public class HouseFoundation
    {
        private readonly ITransformGrid _transformGrid;
        private readonly IAnimalCounter _animalCounter;
        private readonly IStaticDataService _staticData;

        public HouseFoundation(IStaticDataService staticData, ITransformGrid transformGrid, IAnimalCounter animalCounter)
        {
            _staticData = staticData;
            _transformGrid = transformGrid;
            _animalCounter = animalCounter;
        }
    }
}