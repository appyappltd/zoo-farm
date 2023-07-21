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
    public class PlayerInteraction : ObserverTargetExit<Hero, TriggerObserverExit>
    {
        [SerializeField] private float _interactionDelay;
        [SerializeField] private bool _isValidate;

        [SerializeField] [ShowIf("_isValidate")]
        private InterfaceReference<IInteractionValidator, MonoBehaviour> _interactionValidator;

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

        private void OnValidate()
        {
            if (Mathf.Approximately(_interactionDelay, _prevDelayValue))
                return;

            OnAwake();
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            _timerOperator.Pause();
        }

        private void OnDelayPassed() =>
            Interacted.Invoke(_cashedHero);

        protected override void OnTargetEntered(Hero hero)
        {
            if (_isValidate)
            {
                if (_interactionValidator.Value.IsValid(hero.Inventory))
                {
                    InvokeEntered(hero);
                    return;
                }
                
                Rejected.Invoke();
            }
            else
                InvokeEntered(hero);
        }

        private void InvokeEntered(Hero hero)
        {
            _cashedHero = hero;
            _timerOperator.Restart();
            Entered.Invoke(hero);
        }

        protected override void OnTargetExited(Hero hero)
        {
            _timerOperator.Pause();
            Canceled.Invoke();
        }
    }
}