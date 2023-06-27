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

        private void OnDestroy() =>
            _compositeDisposable.Dispose();

        public void Construct(IAnimalsService animalsService)
        {
            _animalsService = animalsService;
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
            foreach (var panel in _releasePanels)
                for (int animalIndex = 0; animalIndex < panel.ReleaseAnimalCount.Value; animalIndex++)
                    _animalsService.Release(panel.AnimalType);
        }

        private void InitPanel(ReleaseAnimalPanel panel)
        {
            AnimalCountData countData = _animalsService.GetAnimalsCount(panel.AnimalType);

            Debug.Log(countData);
            
            if (countData.Total <= 0)
            {
                panel.gameObject.SetActive(false);
                return;
            }
            
            panel.Construct(countData);
            _compositeDisposable.Add(
                panel.ReleaseAnimalCount
                    .Then((UpdateTotalReleaseCount)));
        }

        private void UpdateTotalReleaseCount(int prev, int curr)
        {
            _totalReleaseAnimal += curr - prev;
            _releaseButtonText.SetText(ReleaseAnimalsText, _totalReleaseAnimal);
        }
    }
}