using UnityEngine;
using UnityEngine.Events;

namespace Logic.Translators
{
    public class TranslatableObserver : MonoBehaviour
    {
        [SerializeField] private TranslatableAgent _translatableAgent;

        [SerializeField] private UnityEvent _onBegin;
        [SerializeField] private UnityEvent _onEnd;

        private void OnEnable()
        {
            _translatableAgent.MainTranslatable.End += OnEndTranslate;
            _translatableAgent.MainTranslatable.Begin += OnBeginTranslate;
        }

        private void OnDisable()
        {
            _translatableAgent.MainTranslatable.End -= OnEndTranslate;
            _translatableAgent.MainTranslatable.Begin -= OnBeginTranslate;
        }

        private void OnEndTranslate(ITranslatable _) =>
            _onEnd.Invoke();

        private void OnBeginTranslate(ITranslatable _) =>
            _onBegin.Invoke();
    }
}