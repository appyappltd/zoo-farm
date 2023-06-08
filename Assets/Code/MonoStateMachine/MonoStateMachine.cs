using System;
using System.Collections.Generic;
using UnityEngine;

namespace MonoStateMachine
{
    public abstract class MonoStateMachine : MonoBehaviour
    {
        private Dictionary<Type, IMonoState> _allBehaviors;
        private IMonoState _currentBehavior;

        private void Start()
        {
            InitStates();
            InitTransitions();
            SetDefaultState();
        }

        protected abstract void InitTransitions();

        protected abstract void SetDefaultState();

        protected abstract Dictionary<Type, IMonoState> GetStates();

        public void Enter<TState>() where TState : class, IMonoState
        {
            TState behavior = ChangeState<TState>();
            behavior.EnterBehavior();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedMonoState<TPayload>
        {
            TState behavior = ChangeState<TState>();
            behavior.EnterBehavior(payload);
        }

        private TState ChangeState<TState>() where TState : class, IMonoState
        {
            _currentBehavior?.ExitBehavior();
            TState behavior = GetState<TState>();
            _currentBehavior = behavior;

            Debug.Log($"Enter State {typeof(TState).Name}");
            
            return behavior;
        }

        private TState GetState<TState>() where TState : class, IMonoState =>
            _allBehaviors[typeof(TState)] as TState;
        
        private void InitStates() =>
            _allBehaviors = GetStates();
    }
} 