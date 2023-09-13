using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper;
using DelayRoutines;
using Logic.Interactions.Validators;
using NaughtyAttributes;
using Observer;
using UnityEngine;

namespace Logic.Interactions
{
    [RequireComponent(typeof(TimerOperator))]
    public class InteractionZone<T> : ObserverTargetExit<T, TriggerObserverExit>, IInteractionZone
    {
        private readonly HashSet<Action> _interactionSubs = new HashSet<Action>();
        private readonly List<T> _awaitingUnlock = new List<T>();

        [SerializeField] private TimerOperator _timerOperator;
        [SerializeField] private float _interactionDelay;
        [SerializeField] private bool _isLooped;
        [SerializeField] private bool _isValidate;

        [SerializeField] [ShowIf(nameof(_isValidate))]
        private ValidationMode _validationMode;

        [SerializeField] [ShowIf("_isValidate")]
        private InterfaceReference<IInteractionValidator, MonoBehaviour>[] _interactionValidators;

        private bool _isLocked;
        private RoutineSequence _waitToUnlock;
        private T _cashedTarget;

#if UNITY_EDITOR
        private float _prevDelayValue;
#endif

        public event Action<T> Interacted = _ => { };

        event Action IInteractionZone.Interacted
        {
            add => _interactionSubs.Add(value);
            remove => _interactionSubs.Remove(value);
        }

        public event Action Entered = () => { };
        public event Action Canceled = () => { };
        public event Action Rejected = () => { };

        public float InteractionDelay => _interactionDelay;

        protected override void OnAwake()
        {
            _timerOperator ??= GetComponent<TimerOperator>();
            _timerOperator.SetUp(_interactionDelay, OnDelayPassed);

            Interacted += NotifyAllHashInteractionSubs;
        }

        private void OnDestroy() =>
            _waitToUnlock?.Kill();

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Mathf.Approximately(_interactionDelay, _prevDelayValue))
                return;

            OnAwake();
        }

#endif

        public void Activate() =>
            enabled = true;

        public void Deactivate()
        {
            Canceled.Invoke();
            enabled = false;
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            _isLocked = false;
            StopDelayedInteraction();
        }

        protected override void OnTargetEntered(T target)
        {
            if (_isLocked)
            {
                _awaitingUnlock.Add(target);
                return;
            }

            TryValidateEnter(target);
        }

        private void TryValidateEnter(T hero)
        {
            if (Validate(hero))
            {
                PassEnter(hero);
            }
            else
            {
                if (_validationMode == ValidationMode.PassThrough)
                    InvokeEntered(hero);
                else
                    Rejected.Invoke();
            }
        }

        protected override void OnTargetExited(T target)
        {
            if (_awaitingUnlock.Contains(target))
                _awaitingUnlock.Remove(target);

            if (target.Equals(_cashedTarget))
            {
                _isLocked = false;
                Canceled.Invoke();
                StopDelayedInteraction();
                
                if (_awaitingUnlock.Any())
                    TryValidateEnter(_awaitingUnlock.First());
            }
        }

        private void OnDelayPassed()
        {
            if (_isLooped)
            {
                Interacted.Invoke(_cashedTarget);

                if (Validate(_cashedTarget))
                {
                    InvokeDelayedInteraction();
                }
                else
                {
                    Rejected.Invoke();
                }
            }
            else
            {
                Interacted.Invoke(_cashedTarget);
            }
        }

        private bool Validate(T target)
        {
            if (_isValidate == false)
                return true;

            bool isAllValid = true;

            for (var index = 0; index < _interactionValidators.Length; index++)
                isAllValid &= _interactionValidators[index].Value.IsValid(target);

            return isAllValid;
        }

        private void PassEnter(T hero)
        {
            InvokeDelayedInteraction();
            InvokeEntered(hero);
        }

        private void InvokeEntered(T hero)
        {
            _isLocked = true;
            _cashedTarget = hero;
            Entered.Invoke();
        }

        private void InvokeDelayedInteraction() =>
            _timerOperator.Restart();

        private void StopDelayedInteraction() =>
            _timerOperator.Pause();

        private void NotifyAllHashInteractionSubs(T _)
        {
            foreach (Action action in _interactionSubs)
                action.Invoke();
        }
    }
}