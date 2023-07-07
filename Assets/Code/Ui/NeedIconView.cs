using Observables;
using Progress;
using Tools;
using Ui.Elements;
using UnityEngine;

namespace Ui
{
    public class NeedIconView : MonoBehaviour
    {
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        [Header("Component References")]
        [SerializeField] [RequireInterface(typeof(IProgressBarProvider))] private MonoBehaviour _barHolder;
        [SerializeField] private TextSetter _needText;

        private IProgressBarProvider BarProvider => (IProgressBarProvider) _barHolder;

        private void Start()
        {
            Transform selfTransform = transform;
            selfTransform.forward = Camera.main.transform.forward;

            Construct(BarProvider.ProgressBarView.Current, BarProvider.ProgressBarView.Max);

            BarProvider.ProgressBarView.Empty += ActivateView;
            BarProvider.ProgressBarView.Full += DeactivateView;
        }

        private void OnDestroy()
        {
            _compositeDisposable.Dispose();
            
            BarProvider.ProgressBarView.Empty -= ActivateView;
            BarProvider.ProgressBarView.Full -= DeactivateView;
        }

        public void Construct(IObservable<float> variable, float max)
        {
            UpdateText(variable.Value, max);
            
            _compositeDisposable
                .Add(variable
                    .Then(value => UpdateText(value, max)));
        }

        private void DeactivateView() =>
            gameObject.SetActive(false);

        private void ActivateView() =>
            gameObject.SetActive(true);

        private void UpdateText(float value, float max) =>
            _needText.SetText($"{Mathf.Round(value)}/{max}");
    }
} 