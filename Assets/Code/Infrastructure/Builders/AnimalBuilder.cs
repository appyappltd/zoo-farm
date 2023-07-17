using Data.ItemsData;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
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

        public void Build(Animal animal, AnimalItemData animalItemData)
        {
            //TODO: заменить реализацию хеш кода на уникальный id и добавить статик дату для животного
            AnimalId animalId = new AnimalId(animalItemData.Type, animal.GetHashCode());
            animal.Construct(animalId, animalItemData.BeginStats, _staticDataService);
            _animalsService.Register(animal);
        }
    }
}