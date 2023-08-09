using DelayRoutines;
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

        [Space] [Header("Settings")]
        [SerializeField] private bool _isDeactivateOnFull = true;
        [SerializeField] private float _delayBeforeDeactivate = 2f;

        private IProgressBarView _barView;
        private DelayRoutine _deactivateDelay;

        private void OnDestroy()
        {
            _compositeDisposable.Dispose();

            if (_isDeactivateOnFull)
            {
                _deactivateDelay.Kill();
                _barView.Empty -= ActivateView;
                _barView.Full -= DeactivateView;
            }
        }

        public void Construct(IProgressBarView progressBar)
        {
            UpdateText(progressBar.Current.Value, progressBar.Max);

            _compositeDisposable
                .Add(progressBar.Current
                    .Then(value => UpdateText(value, progressBar.Max)));

            if (_isDeactivateOnFull)
            {
                _deactivateDelay = new DelayRoutine();
                _deactivateDelay
                    .WaitForSeconds(_delayBeforeDeactivate)
                    .Then(() => gameObject.SetActive(false));
                
                progressBar.Empty += ActivateView;
                progressBar.Full += DeactivateView;
            }

            Transform selfTransform = transform;
            selfTransform.forward = Camera.main.transform.forward;

            _barView = progressBar;
        }

        private void DeactivateView() =>
            _deactivateDelay.Play();

        private void ActivateView() =>
            gameObject.SetActive(true);

        private void UpdateText(float value, float max) =>
            _needText.SetText($"{Mathf.Round(value)}/{max}");
    }
}