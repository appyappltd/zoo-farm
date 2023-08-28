using System;
using System.Collections.Generic;
using NTC.Global.Cache;
using Observables;
using StateMachineBase.States;

namespace StateMachineBase
{
    public abstract class StateMachine : MonoCache
    {
        public readonly Observable<Type> CurrentStateType = new Observable<Type>(typeof(Idle));
        private readonly HashSet<IDisposable> _disposable = new HashSet<IDisposable>();

        private State _currentState;
        private State _initialState;

        private void OnDestroy()
        {
           Cleanup();
        }

        public void Stop() =>
            enabled = false;

        public void Play()
        {
            enabled = true;

            if (_currentState is null)
                return;

            ChangeState(_initialState);
        }

        protected override void Run() =>
            _currentState.Update();

        protected void Cleanup()
        {
            foreach (var transition in _disposable)
                transition.Dispose();
        }

        protected void ForceState(State state)
        {
            if (enabled)
                ChangeState(state);
        }

        protected void Init(State initialState, Dictionary<State, Dictionary<Transition, State>> states)
        {
            foreach ((State state, Dictionary<Transition, State> transitions) in states)
                AddState(state, transitions);

            _initialState = initialState;
            _currentState = initialState;
        }

        protected void AddState(State state, Dictionary<Transition, State> transitions)
        {
            foreach ((Transition key, State value) in transitions)
            {
                key.Callback = () => ChangeState(value);
                state.AddTransition(key);

                if (key is IDisposable disposable)
                    _disposable.Add(disposable);
                    
                if (value is IDisposable disposable1)
                    _disposable.Add(disposable1);
            }
        }
        
        private void ChangeState(State newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
            CurrentStateType.Value = _currentState.GetType();
        }
    }
}