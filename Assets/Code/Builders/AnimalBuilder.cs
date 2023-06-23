using Logic.Animals;
using Logic.AnimalsBehaviour;
using Services.StaticData;

namespace Builders
{
    public class AnimalBuilder
    {
        private readonly IStaticDataService _staticDataService;

        public AnimalBuilder(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void Build(Animal animal)
        {
            AnimalId animalId = new AnimalId(AnimalType.CatB, animal.GetHashCode());
            animal.Construct(animalId, _staticDataService);
        }
    }
}