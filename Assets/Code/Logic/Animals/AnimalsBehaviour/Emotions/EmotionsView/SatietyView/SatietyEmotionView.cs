using Logic.Animals.AnimalsBehaviour.AnimalStats;
using Observables;
using StaticData;
using UnityEngine;

namespace Logic.Animals.AnimalsBehaviour.Emotions.EmotionsView.SatietyView
{
    public class SatietyEmotionView : EmotionView
    {
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        [Header("References")] [SerializeField]
        private StatIndicator _satietyBarIndicator;

        [SerializeField] private SatietyIconsConfig _iconsConfig;

        [Space] [Header("SatietyStages")]
        [SerializeField] private StaticSatietyView _staticSatietyView;
        [SerializeField] private DynamicSatietyBarView _dynamicSatietyBarView;

        private GameObject _active;

        private void Awake()
        {
            _active = _staticSatietyView.gameObject;
            _dynamicSatietyBarView.Construct(_iconsConfig);
        }

        private void OnEnable()
        {
            _compositeDisposable.Add(_satietyBarIndicator.ProgressBar.Current.Then(OnSatietyUpdated));
            OnSatietyUpdated();
        }

        private void OnDisable() =>
            _compositeDisposable.Dispose();

        private void ShowFullIcon()
        {
            _active.SetActive(false);
            _staticSatietyView.Show(_iconsConfig.FullSatiety, 2);
            _active = _staticSatietyView.gameObject;
        }

        private void ShowEmptyIcon()
        {
            _active.SetActive(false);
            _staticSatietyView.Show(_iconsConfig.EmptySatiety, 0);
            _active = _staticSatietyView.gameObject;
        }

        private void ShowDynamicBarIcon(float normalized)
        {
            if (_active != _dynamicSatietyBarView.gameObject)
            {
                _active.SetActive(false);
                _dynamicSatietyBarView.gameObject.SetActive(true);
            }

            _dynamicSatietyBarView.SetFill(normalized);
        }

        private void OnSatietyUpdated()
        {
            float normalized = _satietyBarIndicator.ProgressBar.CurrentNormalized;

            switch (normalized)
            {
                case >= 1f:
                    ShowFullIcon();
                    break;
                case <= 0f:
                    ShowEmptyIcon();
                    break;
                default:
                    ShowDynamicBarIcon(normalized);
                    break;
            }
        }
    }
}