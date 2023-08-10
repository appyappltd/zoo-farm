using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Data.ItemsData;
using Services.Animals;
using Services.StaticData;

namespace Infrastructure.Builders
{
    public class AnimalBuilder
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAnimalsService _animalsService;

        public AnimalBuilder(IStaticDataService staticDataService, IAnimalsService animalsService)
        {
            _staticDataService = staticDataService;
            _animalsService = animalsService;
        }

        public void Build(Animal animal, AnimalItemStaticData staticData)
        {
            //TODO: заменить реализацию хеш кода на уникальный id и добавить статик дату для животного
            AnimalId animalId = new AnimalId(staticData.AnimalType, animal.GetHashCode());
            animal.Construct(animalId, staticData.BeginStats, _staticDataService);
            _animalsService.Register(animal);
        }
    }
}