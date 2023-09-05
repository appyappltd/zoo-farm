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
            _translatableAgent.Main.End += OnEndTranslate;
            _translatableAgent.Main.Begin += OnBeginTranslate;
        }

        private void OnDisable()
        {
            _translatableAgent.Main.End -= OnEndTranslate;
            _translatableAgent.Main.Begin -= OnBeginTranslate;
        }

        private void OnEndTranslate(ITranslatable _) =>
            _onEnd.Invoke();

        private void OnBeginTranslate(ITranslatable _) =>
            _onBegin.Invoke();
    }
}