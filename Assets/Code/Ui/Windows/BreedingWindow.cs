using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Animals;
using NTC.Global.System;
using Services.Animals;
using Services.StaticData;
using Ui.Elements;
using Ui.Factory;
using UnityEngine;

namespace Ui.Windows
{
    public class BreedingWindow : WindowBase
    {
        private const string ChoosingAnimals = "Кто заведёт потомство?";
        private const string NoAnimalsToChoose = "Сейчас некому завести потомство :(";
        
        [SerializeField] private Transform _panelsParent;
        [SerializeField] private TextSetter _descriptionText;
        [SerializeField] private GameObject _defaultPanel;

        private Action<AnimalType> _onChoseCallback;

        public void SetOnChoseCallback(Action<AnimalType> callback) =>
            _onChoseCallback = callback;
        
        public void Construct(IAnimalsService animalsService, UIFactory uiFactory, IStaticDataService staticData)
        {
            List<AnimalType> animalTypes = animalsService.GetBreedingReady().ToList();
            
            ChoosePanels(animalTypes);

            foreach (AnimalType animal in animalTypes)
                ConstructPanel(uiFactory, staticData, animal);
        }

        private void ConstructPanel(UIFactory uiFactory, IStaticDataService staticData, AnimalType animal)
        {
            ChoseAnimalPanel panel = uiFactory.CreateChoseAnimalPanel(_panelsParent);
            panel.Construct(staticData.IconByAnimalType(animal), () =>
            {
                _onChoseCallback.Invoke(animal);
                CloseWindow();
            });
        }

        private void ChoosePanels(List<AnimalType> animalTypes)
        {
            if (animalTypes.Any())
                ShowChoosingPanel();
            else
                ShowNoAnimalsPanel();
        }

        private void ShowNoAnimalsPanel()
        {
            _descriptionText.SetText(NoAnimalsToChoose);
            _defaultPanel.Enable();
        }

        private void ShowChoosingPanel()
        {
            _descriptionText.SetText(ChoosingAnimals);
            _defaultPanel.Disable();
        }
    }
}