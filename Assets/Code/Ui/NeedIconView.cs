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

        [SerializeField] [RequireInterface(typeof(IProgressBarHolder))] private MonoBehaviour _barHolder;
        [SerializeField] private TextSetter _needText;

        private IProgressBarHolder BarHolder => (IProgressBarHolder) _barHolder; 

        private void Start()
        {
            transform.forward = Camera.main.transform.forward;
            Construct(BarHolder.ProgressBarView.Current, BarHolder.ProgressBarView.Max);
        }

        private void OnDisable() =>
            _compositeDisposable.Dispose();

        public void Construct(IObservable<float> variable, float max)
        {
            UpdateText(variable.Value, max);
            
            _compositeDisposable
                .Add(variable
                    .Then(value => UpdateText(value, max)));
        }

        private void UpdateText(float value, float max) =>
            _needText.SetText($"{Mathf.Round(value)}/{max}");
    }
} 