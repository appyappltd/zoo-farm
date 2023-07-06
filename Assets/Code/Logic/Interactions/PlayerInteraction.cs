using System;
using Observer;
using UnityEngine;

namespace Logic.Interactions
{
    [RequireComponent(typeof(TimerOperator))]
    public class PlayerInteraction : ObserverTargetExit<HeroProvider, TriggerObserverExit>
    {
        [SerializeField] private float _interactionDelay;

#if UNITY_EDITOR
        private float _prevDelayValue;
#endif
        
        [SerializeField] private TimerOperator _timerOperator;
        private HeroProvider _cashedHeroProvider;
        
        public event Action<HeroProvider> Interacted = c => { };
        public event Action<HeroProvider> Entered = c => { };
        public event Action Canceled = () => { };

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
            Interacted.Invoke(_cashedHeroProvider);

        protected override void OnTargetEntered(HeroProvider heroProvider)
        {
            _cashedHeroProvider = heroProvider;
            _timerOperator.Restart();
            Entered.Invoke(heroProvider);
        }

        protected override void OnTargetExited(HeroProvider heroProvider)
        {
            _timerOperator.Pause();
            Canceled.Invoke();
        }
    }
}