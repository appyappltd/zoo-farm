using System;
using Logic.Animals;
using Services.Animals;
using Services.StaticData;
using Ui.Factory;
using UnityEngine;

namespace Ui.Windows
{
    public class BreedingWindow : WindowBase
    {
        [SerializeField] private Transform _panelsParent;

        private Action<AnimalType> _onChoseCallback;

        public void SetOnChoseCallback(Action<AnimalType> callback) =>
            _onChoseCallback = callback;
        
        public void Construct(IAnimalsService animalsService, UIFactory uiFactory, IStaticDataService staticData)
        {
            foreach (AnimalType animal in animalsService.GetBreedingReady())
            {
                ChoseAnimalPanel panel = uiFactory.CreateChoseAnimalPanel(_panelsParent);
                panel.Construct(staticData.IconByAnimalType(animal), () =>
                {
                    _onChoseCallback.Invoke(animal);
                    CloseWindow();
                });
            }
        }
    }
}