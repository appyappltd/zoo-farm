using System.Collections.Generic;
using Logic.Animals;
using Services.StaticData;
using Ui;
using UnityEngine;

namespace Infrastructure.Builders
{
    public class AnimalGoalPanelBuilder
    {
        private readonly IStaticDataService _staticDataService;

        public AnimalGoalPanelBuilder(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public GoalAnimalPanelProvider Build(GameObject providerObject, KeyValuePair<AnimalType, int> countTypePair)
        {
            var provider = providerObject.GetComponent<GoalAnimalPanelProvider>();
            provider.AnimalIcon.sprite = _staticDataService.IconByAnimalType(countTypePair.Key);
            string text = $"0/{countTypePair.Value}";
            provider.CountText.SetText(text);
            return provider;
        }
    }
}