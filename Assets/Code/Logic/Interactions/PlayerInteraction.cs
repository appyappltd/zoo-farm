using System;
using Logic.Player;
using Observer;
using UnityEngine;

namespace Logic.Interactions
{
    [RequireComponent(typeof(TimerOperator))]
    public class PlayerInteraction : ObserverTargetExit<Hero, TriggerObserverExit>
    {
        [SerializeField] private float _interactionDelay;

#if UNITY_EDITOR
        private float _prevDelayValue;
#endif
        
        [SerializeField] private TimerOperator _timerOperator;
        
        private Hero _cashedHero;

        public event Action<Hero> Interacted = c => { };
        public event Action<Hero> Entered = c => { };
        public event Action Canceled = () => { };

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

        private void OnDelayPassed() =>
            Interacted.Invoke(_cashedHero);

        protected override void OnTargetEntered(Hero hero)
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