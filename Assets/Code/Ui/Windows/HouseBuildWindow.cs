using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Services.AnimalHouses;
using Services.StaticData;
using Tools.Comparers;
using Ui.Factory;
using UnityEngine;

namespace Ui.Windows
{
    public class HouseBuildWindow : WindowBase
    {
        [SerializeField] private Transform _panelsParent;

        private Action<AnimalType> _onChoseCallback;

        public void SetOnChoseCallback(Action<AnimalType> callback) =>
            _onChoseCallback = callback;

        public void Construct(IAnimalHouseService houseService, IUIFactory uiFactory, IStaticDataService staticData)
        {
            IEnumerable<IAnimal> houseServiceAnimalsInQueue = houseService.AnimalsInQueue.Select(queue => queue.Animal).Distinct(new AnimalByTypeComparer());
            
            foreach (IAnimal animal in houseServiceAnimalsInQueue)
            {
                ChoseAnimalPanel panel = uiFactory.CreateChoseAnimalPanel(_panelsParent);
                panel.Construct(staticData.IconByAnimalType(animal.AnimalId.Type), () =>
                {
                    _onChoseCallback.Invoke(animal.AnimalId.Type);
                    CloseWindow();
                });
            }
        }
    }
}