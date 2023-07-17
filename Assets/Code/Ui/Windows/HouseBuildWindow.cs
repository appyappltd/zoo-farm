using System;
using Logic.Animals;
using Services.AnimalHouses;
using Services.StaticData;
using Ui.Factory;
using UnityEngine;

namespace Ui.Windows
{
    public class HouseBuildWindow : WindowBase
    {
        [SerializeField] private Transform _panelsParent;

        private IAnimalHouseService _animalHouseService;

        public event Action<AnimalId> HouseChosen = i => { };

        public void Construct(IAnimalHouseService progressService, IUIFactory uiFactory, IStaticDataService staticData)
        {
            _animalHouseService = progressService;

            foreach (AnimalId animalId in _animalHouseService.AnimalsInQueue)
            {
                BuildHousePanel panel = uiFactory.CreateBuildHousePanel(_panelsParent);
                panel.Construct(staticData.IconByAnimalType(animalId.Type), () =>
                {
                    HouseChosen.Invoke(animalId);
                    CloseWindow();
                });
            }
        }
    }
}