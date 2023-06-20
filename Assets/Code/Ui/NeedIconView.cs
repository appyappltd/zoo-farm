using Observables;
using Ui.Elements;
using UnityEngine;

namespace Ui
{
    public class NeedIconView : MonoBehaviour
    {
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        [SerializeField] private TextSetter _textSetter;

        private void Start()
        {
            transform.forward = Camera.main.transform.forward;
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
            _textSetter.SetText($"{value}/{max}");
    }
} 