using System.Collections.Generic;
using NTC.Global.Cache;

namespace StateMachineBase
{
    public abstract class StateMachine : MonoCache
    {
        private State _currentState;
        private State _initialState;

        private void ChangeState(State newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public void Stop() =>
            enabled = false;

        public void Play()
        {
            enabled = true;
        }

        protected override void Run() =>
            _currentState.Update();

        protected void Init(State initialState, Dictionary<State, Dictionary<Transition, State>> states)
        {
            foreach ((State state, Dictionary<Transition, State> dictionary) in states)
            {
                foreach ((Transition key, State value) in dictionary)
                {
                    key.Callback = () => ChangeState(value);
                    state.AddTransition(key);
                }
            }

            _initialState = initialState;
            ChangeState(initialState);
        }
    }
}