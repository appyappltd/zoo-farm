using Logic.Animals;
using Logic.LevelGoals;
using Services.StaticData;
using UnityEngine;

namespace Infrastructure.Builders
{
    public class ReleaseInteractionZoneBuilder
    {
        private readonly IStaticDataService _staticDataService;

        public ReleaseInteractionZoneBuilder(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public ReleaseInteractionProvider Build(GameObject zoneObject, AnimalType animalType)
        {
            ReleaseInteractionProvider provider = zoneObject.GetComponent<ReleaseInteractionProvider>();
            provider.AnimalIcon.sprite = _staticDataService.IconByAnimalType(animalType);
            return provider;
        }
    }
}