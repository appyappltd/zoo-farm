using System.Collections.Generic;
using System.Linq;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Observables;
using Services.Animals;
using Services.StaticData;
using TMPro;
using Tools;
using Tools.Comparers;
using Ui.Factory;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Ui.Windows
{
    public class AnimalReleaseWindow : WindowBase
    {
        private const string ReleaseAnimalsText = "Отпустить животных: {0}?";
        private const string NoReleaseAnimalsText = "Сейчас некого отпустить";
        private const string AnimalNotSelectedText = "Животное не выбрано";

        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private readonly List<ReleaseAnimalPanel> _releasePanels = new List<ReleaseAnimalPanel>();

        [SerializeField] private Transform _panelsParent;
        [SerializeField] private Button _releaseButton;
        [SerializeField] private TextMeshProUGUI _releaseButtonText;
        [SerializeField] private UiGrayScaleFilter _releaseButtonFilter;

        private IAnimalsService _animalsService;
        private int _totalReleaseAnimal;

        protected override void Cleanup() =>
            _compositeDisposable.Dispose();

        public void Construct(IAnimalsService animalsService, IUIFactory uiFactory, IStaticDataService staticData)
        {
            _animalsService = animalsService;

            IEnumerable<IAnimal> releaseReadyAnimals = _animalsService.GetReleaseReady();

            foreach (IAnimal animal in releaseReadyAnimals)
            {
                ReleaseAnimalPanel panel = uiFactory.CreateReleaseAnimalPanel(_panelsParent);
                AnimalType animalIdType = animal.AnimalId.Type;
                InitPanel(panel, animalIdType, staticData.IconByAnimalType(animalIdType));
                _releasePanels.Add(panel);
            }

            SetButtonGrey();
            _releaseButtonText.SetText(_animalsService.ReleaseReadyAnimalCount > 0
                ? AnimalNotSelectedText
                : NoReleaseAnimalsText);

            _releaseButton.onClick.AddListener(OnClickRelease);
        }

        private void OnClickRelease()
        {
            foreach (ReleaseAnimalPanel panel in _releasePanels)
                for (int animalIndex = 0; animalIndex < panel.ReleaseAnimalCount.Value; animalIndex++)
                    _animalsService.Release(panel.AnimalType);

            CloseWindow();
        }

        private void InitPanel(ReleaseAnimalPanel panel, AnimalType animalType, Sprite icon)
        {
            AnimalCountData countData = _animalsService.AnimalCounter.GetAnimalCountData(animalType);
            _releaseButton.interactable = false;

            if (countData.Total <= 0)
            {
                panel.gameObject.SetActive(false);
                return;
            }


            panel.Construct(countData, animalType, icon);
            _compositeDisposable.Add(
                panel.ReleaseAnimalCount
                    .Then(OnUpdateTotalReleaseCount));
        }

        private void OnUpdateTotalReleaseCount(int prev, int curr)
        {
            _totalReleaseAnimal += curr - prev;

            if (_totalReleaseAnimal == 0)
            {
                _releaseButtonText.SetText(AnimalNotSelectedText);
                SetButtonGrey();
            }

            SetButtonColorful();
            _releaseButtonText.SetText(ReleaseAnimalsText, _totalReleaseAnimal);
            _releaseButton.interactable = _totalReleaseAnimal > 0;
        }

        private void SetButtonColorful() =>
            _releaseButtonFilter.SetEffectAmount(0);

        private void SetButtonGrey() =>
            _releaseButtonFilter.SetEffectAmount(1);
    }
}