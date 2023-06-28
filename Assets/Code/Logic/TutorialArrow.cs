using System;
using Logic.Interactions;
using Logic.Translators;
using Tools.Extension;
using Tutorial;
using UnityEngine;

namespace Logic
{
    [RequireComponent(typeof(TriggerObserver))]
    [RequireComponent(typeof(RunTranslator))]
    public class TutorialArrow : MonoBehaviour, ITutorialTrigger
    {
        [SerializeField] private RunTranslator _translator;
        [SerializeField] private TranslatableAgent _translatableAgent;

        private TriggerObserver _triggerObserver;

        public event Action Triggered = () => { };

        private void Awake()
        {
            _triggerObserver = GetComponent<TriggerObserver>();
        }

        private void Start()
        {
            _translator.AddTranslatable(_translatableAgent.MainTranslatable);
            _triggerObserver.Enter += OnEnter;
        }

        private void OnEnter(GameObject _) =>
            Triggered.Invoke();

        public void Move(Vector3 to)
        {
            gameObject.SetActive(true);
            transform.position = to.ChangeY(transform.position.y);
        }

        public void Hide() =>
            gameObject.SetActive(false);
    }
}