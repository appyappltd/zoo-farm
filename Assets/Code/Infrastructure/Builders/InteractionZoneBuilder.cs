using System;
using Logic.Interactions;
using Logic.Interactions;
using Logic.Animals;
using Logic.Houses;
using Logic.LevelGoals;
using Services.Animals;
using Services.StaticData;
using UnityEngine;

namespace Code.Infrastructure.Builders
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
                case ChoseHouseInteractionProvider choseHouse:
                    Build(choseHouse, animalType);
                    break;
            }
            
            return provider;
        }

        private void Build(ReleaseInteractionProvider provider, AnimalType animalType)
        {
            provider.ReleaseIcon.Construct(_animalsService, _staticDataService, animalType);
            provider.Validator.Construct(_animalsService, animalType);
        }

        private void Build(ChoseHouseInteractionProvider provider, AnimalType animalType)
        {
            provider.AnimalIcon.sprite = _staticDataService.IconByAnimalType(animalType);
        }
    }
}