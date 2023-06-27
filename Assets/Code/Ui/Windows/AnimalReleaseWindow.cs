using Observables;
using Services.Animals;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Ui.Windows
{
    public class AnimalReleaseWindow : WindowBase
    {
        private const string ReleaseAnimalsText = "Отпустить {0} животных(-ое)?";

        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        
        [SerializeField] private ReleaseAnimalPanel[] _releasePanels;
        [SerializeField] private Button _releaseButton;
        [SerializeField] private TextMeshProUGUI _releaseButtonText;

        private IAnimalsService _animalsService;
        private int _totalReleaseAnimal;

        protected override void Cleanup() =>
            _compositeDisposable.Dispose();

        public void Construct(IAnimalsService animalsService)
        {
            _animalsService = animalsService;
            Debug.Log(_animalsService);
        }

        protected override void Initialize()
        {
            _releasePanels = GetComponentsInChildren<ReleaseAnimalPanel>();
            
            foreach (ReleaseAnimalPanel panel in _releasePanels)
            {
                InitPanel(panel);
            }
            
            _releaseButton.onClick.AddListener(OnClickRelease);
        }

        private void OnClickRelease()
        {
            foreach (ReleaseAnimalPanel panel in _releasePanels)
                for (int animalIndex = 0; animalIndex < panel.ReleaseAnimalCount.Value; animalIndex++)
                    _animalsService.Release(panel.AnimalType);
            
            CloseWindow();
        }

        private void InitPanel(ReleaseAnimalPanel panel)
        {
            AnimalCountData countData = _animalsService.GetAnimalsCount(panel.AnimalType);
            _releaseButton.interactable = false;

            if (countData.Total <= 0)
            {
                panel.gameObject.SetActive(false);
                return;
            }
            
            panel.Construct(countData);
            _compositeDisposable.Add(
                panel.ReleaseAnimalCount
                    .Then(OnUpdateTotalReleaseCount));
        }

        private void OnUpdateTotalReleaseCount(int prev, int curr)
        {
            _totalReleaseAnimal += curr - prev;
            _releaseButtonText.SetText(ReleaseAnimalsText, _totalReleaseAnimal);
            _releaseButton.interactable = _totalReleaseAnimal > 0;
        }
    }
}