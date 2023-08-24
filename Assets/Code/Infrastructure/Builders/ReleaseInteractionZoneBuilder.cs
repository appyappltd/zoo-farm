using Logic.Animals;
using Logic.LevelGoals;
using Services.Animals;
using Services.StaticData;
using UnityEngine;

namespace Code.Infrastructure.Builders
{
    public class ReleaseInteractionZoneBuilder
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAnimalsService _animalsService;

        public ReleaseInteractionZoneBuilder(IAnimalsService animalsService, IStaticDataService staticDataService)
        {
            _animalsService = animalsService;
            _staticDataService = staticDataService;
        }

        public ReleaseInteractionProvider Build(GameObject providerObject, AnimalType animalType)
        {
            var provider = providerObject.GetComponent<ReleaseInteractionProvider>();
            provider.ReleaseIcon.Construct(_animalsService, _staticDataService, animalType);
            provider.Validator.Construct(_animalsService, animalType);
            return provider;
        }
    }
}