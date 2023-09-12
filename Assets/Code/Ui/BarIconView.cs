using DelayRoutines;
using NTC.Global.System;
using Observables;
using Progress;
using Ui.Elements;
using UnityEngine;

namespace Ui
{
    public class BarIconView : MonoBehaviour
    {
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        [Header("References")]
        [SerializeField] private TextSetter _needText;
        [SerializeField] private SpriteRenderer _icon;

        [Space] [Header("Settings")]
        [SerializeField] private bool _isDeactivateOnFull = true;
        [SerializeField] [Min(0)] private float _delayBeforeDeactivate = 2f;

        private IProgressBarView _barView;
        private RoutineSequence _deactivate;

        private void OnDestroy()
        {
            _compositeDisposable.Dispose();

            if (_isDeactivateOnFull && _barView != null)
            {
                _deactivate.Kill();
                _barView.Empty -= ActivateView;
                _barView.Full -= DeactivateView;
            }
        }

        public void Construct(IProgressBarView progressBar, Sprite icon)
        {
            _icon.sprite = icon;
            Construct(progressBar);
        }
        
        public void Construct(IProgressBarView progressBar)
        {
            UpdateText(progressBar.Current.Value, progressBar.Max);

            _compositeDisposable
                .Add(progressBar.Current
                    .Then(value => UpdateText(value, progressBar.Max)));

            if (_isDeactivateOnFull)
            {
                _deactivate = new RoutineSequence();
                _deactivate
                    .WaitForSeconds(_delayBeforeDeactivate)
                    .Then(() => gameObject.Disable());
                
                progressBar.Empty += ActivateView;
                progressBar.Full += DeactivateView;
            }

            _barView = progressBar;
        }

        private void DeactivateView() =>
            _deactivate.Play();

        private void ActivateView() =>
            gameObject.Enable();

        private void UpdateText(float value, float max) =>
            _needText.SetText($"{Mathf.Round(value)}/{max}");
    }
}