using Logic.AnimalsBehaviour.States;
using MonoStateMachine;
using UnityEngine;

namespace Logic.AnimalsBehaviour.Transitions
{
    public class EatToIdleTransition : Transition
    {
        [SerializeField] private ProgressBarIndicator _satiety;

        protected override void OnEnabled() =>
            _satiety.ProgressBar.Full += MoveToIdleState;

        protected override void OnDisabled() =>
            _satiety.ProgressBar.Full -= MoveToIdleState;

        private void MoveToIdleState()
        {
            StateMachine.Enter<AnimalIdleState>();
        }
    }
}