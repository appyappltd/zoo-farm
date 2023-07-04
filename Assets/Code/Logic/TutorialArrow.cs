using System;
using Logic.Interactions;
using Logic.Translators;
using Tools.Extension;
using Tutorial;
using UnityEngine;

namespace Logic
{
    [RequireComponent(typeof(RunTranslator))]
    public class TutorialArrow : MonoBehaviour, ITutorialTrigger
    {
        [SerializeField] private RunTranslator _translator;
        [SerializeField] private TranslatableAgent _translatableAgent;
        [SerializeField] private PlayerInteraction _playerInteraction;

        public event Action Triggered = () => { };

        private void Start()
        {
            _translator.AddTranslatable(_translatableAgent.MainTranslatable);
            _playerInteraction.Interacted += OnEnter;
        }

        private void OnDestroy() =>
            _playerInteraction.Interacted -= OnEnter;

        private void OnEnter(HeroProvider _) =>
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