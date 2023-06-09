using Logic.AnimalsBehaviour.AnimalStats;
using Logic.AnimalsBehaviour.States;
using MonoStateMachine;
using UnityEngine;

namespace Logic.AnimalsBehaviour.Transitions
{
    public class RestToIdleTransition : Transition
    {
        [SerializeField] private ProgressBarIndicator _peppiness;

        protected override void OnEnabled() =>
            _peppiness.ProgressBar.Full += MoveToIdleState;

        protected override void OnDisabled() =>
            _peppiness.ProgressBar.Full -= MoveToIdleState;

        private void MoveToIdleState()
        {
            Debug.Log("onFull");
            StateMachine.Enter<AnimalIdleState>();
        }
    }
}