using Logic.Animals;
using Logic.Foods.FoodSettings;
using Logic.Interactions;
using Services.Animals;
using Services.StaticData;
using UnityEngine;

namespace Infrastructure.Builders
{
    public class InteractionZoneBuilder
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAnimalsService _animalsService;

        public InteractionZoneBuilder(IAnimalsService animalsService, IStaticDataService staticDataService)
        {
            _animalsService = animalsService;
            _staticDataService = staticDataService;
        }

        public TZone Build<TZone>(GameObject providerObject,
            AnimalType animalType) where TZone : IInteractionZoneProvider
        {
            var provider = providerObject.GetComponent<TZone>();

            switch (provider)
            {
                case ReleaseInteractionProvider release:
                    Build(release, animalType);
                    break;
                case ChoseInteractionProvider choseHouse:
                    Build(choseHouse, animalType);
                    break;
            }
            
            return provider;
        }

        public TZone Build<TZone>(GameObject providerObject,
            FoodId foodType) where TZone : IInteractionZoneProvider
        {
            var provider = providerObject.GetComponent<TZone>();
            Build(provider as ChoseInteractionProvider, foodType);
            return provider;
        }
        
        private void Build(ReleaseInteractionProvider provider, AnimalType animalType)
        {
            provider.ReleaseIcon.Construct(_animalsService, _staticDataService, animalType);
            provider.Validator.Construct(_animalsService, animalType);
        }

        private void Build(ChoseInteractionProvider provider, AnimalType animalType) =>
            provider.AnimalIcon.sprite = _staticDataService.IconByAnimalType(animalType);

        private void Build(ChoseInteractionProvider provider, FoodId foodId) =>
            provider.AnimalIcon.sprite = _staticDataService.IconByFoodType(foodId);
    }
}