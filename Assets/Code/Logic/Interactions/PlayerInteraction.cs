using System;
using AYellowpaper;
using Logic.Interactions.Validators;
using Logic.Player;
using NaughtyAttributes;
using Observer;
using UnityEngine;

namespace Logic.Interactions
{
    [RequireComponent(typeof(TimerOperator))]
    public sealed class PlayerInteraction : ObserverTargetExit<Hero, TriggerObserverExit>
    {
        [SerializeField] private float _interactionDelay;
        [SerializeField] private bool _isValidate;

        [SerializeField] [ShowIf("_isValidate")]
        private InterfaceReference<IInteractionValidator, MonoBehaviour>[] _interactionValidators;

#if UNITY_EDITOR
        private float _prevDelayValue;
#endif

        [SerializeField] private TimerOperator _timerOperator;

        private Hero _cashedHero;

        public event Action<Hero> Interacted = c => { };
        public event Action<Hero> Entered = c => { };
        public event Action Canceled = () => { };
        public event Action Rejected = () => { };

        public float InteractionDelay => _interactionDelay;

        protected override void OnAwake()
        {
            _timerOperator ??= GetComponent<TimerOperator>();
            _timerOperator.SetUp(_interactionDelay, OnDelayPassed);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Mathf.Approximately(_interactionDelay, _prevDelayValue))
                return;

            OnAwake();
        }
#endif

        protected override void OnDisabled()
        {
            base.OnDisabled();
            _timerOperator.Pause();
        }

        protected override void OnTargetEntered(Hero hero)
        {
            if (_isValidate)
                Validate(hero);
            else
                InvokeEntered(hero);
        }

        private void Validate(Hero hero)
        {
            bool isAllValid = true;

            for (var index = 0; index < _interactionValidators.Length; index++)
                isAllValid &= _interactionValidators[index].Value.IsValid(hero.Inventory);

            if (isAllValid)
                InvokeEntered(hero);
            else
                Rejected.Invoke();
        }

        protected override void OnTargetExited(Hero hero)
        {
            _timerOperator.Pause();
            Canceled.Invoke();
        }

        private void OnDelayPassed()
        {
            Interacted.Invoke(_cashedHero);
        }

        private void InvokeEntered(Hero hero)
        {
            _cashedHero = hero;
            _timerOperator.Restart();
            Entered.Invoke(hero);
        }
    }
}