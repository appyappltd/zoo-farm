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
        [SerializeField] [RequireInterface(typeof(IProgressBarHolder))] private MonoBehaviour _barHolder;
        [SerializeField] private TextSetter _needText;

        private IProgressBarHolder BarHolder => (IProgressBarHolder) _barHolder;

        private void Start()
        {
            Transform selfTransform = transform;
            selfTransform.forward = Camera.main.transform.forward;

            Construct(BarHolder.ProgressBarView.Current, BarHolder.ProgressBarView.Max);

            BarHolder.ProgressBarView.Empty += ActivateView;
            BarHolder.ProgressBarView.Full += DeactivateView;
        }

        private void OnDestroy()
        {
            _compositeDisposable.Dispose();
            
            BarHolder.ProgressBarView.Empty -= ActivateView;
            BarHolder.ProgressBarView.Full -= DeactivateView;
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