using System.Collections.Generic;
using UnityEngine;

namespace StateMachineBase
{
    public abstract class StateMachine : MonoBehaviour
    {
        private State _currentState;

        private void ChangeState(State newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        private void Update() => _currentState.Update();

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

            ChangeState(initialState);
        }
    }
}