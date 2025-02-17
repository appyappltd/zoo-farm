using Data.ItemsData;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Services.Animals;
using Services.StaticData;
using UnityEngine;

namespace Infrastructure.Builders
{
    public class AnimalBuilder
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAnimalsService _animalsService;

        private int _animalsCount;
        
        public AnimalBuilder(IStaticDataService staticDataService, IAnimalsService animalsService)
        {
            _staticDataService = staticDataService;
            _animalsService = animalsService;
        }

        public Animal Build(GameObject animalObject, AnimalItemStaticData staticData)
        {
            _animalsCount++;
            Animal animal = animalObject.GetComponent<Animal>();
            AnimalId animalId = new AnimalId(staticData.AnimalType, _animalsCount, staticData.EdibleFood);
            animal.Construct(animalId, staticData.BeginStats, _staticDataService);
            _animalsService.Register(animal);
            return animal;
        }
    }
}