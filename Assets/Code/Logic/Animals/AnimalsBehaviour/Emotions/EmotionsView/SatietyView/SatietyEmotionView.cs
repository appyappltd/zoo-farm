using Logic.Animals.AnimalsBehaviour.AnimalStats;
using NTC.Global.System;
using Observables;
using StaticData;
using UnityEngine;

namespace Logic.Animals.AnimalsBehaviour.Emotions.EmotionsView.SatietyView
{
    public class SatietyEmotionView : EmotionView
    {
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        [Header("References")]
        [SerializeField] private StatIndicator _satietyBarIndicator;
        [SerializeField] private HappinessFactor _happinessFactor;

        [Space] [Header("StaticData")]
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
            SwitchActive(_staticSatietyView.gameObject);
            _staticSatietyView.Show(_iconsConfig.FullSatiety, _happinessFactor.Factor);
        }

        private void ShowEmptyIcon()
        {
            SwitchActive(_staticSatietyView.gameObject);
            _staticSatietyView.Show(_iconsConfig.EmptySatiety, 0);
        }

        private void ShowDynamicBarIcon(float normalized)
        {
            SwitchActive(_dynamicSatietyBarView.gameObject);
            _dynamicSatietyBarView.SetFill(normalized);
        }

        private void SwitchActive(GameObject to)
        {
            if (_active != to)
            {
                _active.Disable();
                _active = to;
                _active.Enable();
            }
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