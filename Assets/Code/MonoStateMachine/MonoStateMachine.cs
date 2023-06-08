using System;
using System.Collections;
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
            StartCoroutine(ChangeState<TState>((state => state.EnterBehavior())));
            // TState behavior = ChangeState<TState>();
            // behavior.EnterBehavior();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedMonoState<TPayload>
        {
            StartCoroutine(ChangeState<TState>((state => state.EnterBehavior(payload))));
            
            // TState behavior = ChangeState<TState>();
            // behavior.EnterBehavior(payload);
        }

        // private TState ChangeState<TState>() where TState : class, IMonoState
        // {
        //     TState behavior = GetState<TState>();
        //     _currentBehavior = behavior;
        //
        //     Debug.Log($"Enter State {typeof(TState).Name}");
        //     
        //     return behavior;
        // }

        private TState GetState<TState>() where TState : class, IMonoState =>
            _allBehaviors[typeof(TState)] as TState;
        
        private void InitStates() =>
            _allBehaviors = GetStates();

        private IEnumerator ChangeState<TState>(Action<TState> result) where TState : class, IMonoState
        {
            _currentBehavior?.ExitBehavior();
            TState behavior = GetState<TState>();
            _currentBehavior = behavior;
            yield return null;

            Debug.Log($"Enter State {typeof(TState).Name}");

            result.Invoke(behavior);
        }
    }
} 