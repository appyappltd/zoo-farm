using Logic.AnimalsBehaviour.States;
using MonoStateMachine;
using UnityEngine;

namespace Logic.AnimalsBehaviour.Transitions
{
    public class RestToIdleTransition : Transition
    {
        [SerializeField] private ProgressBarIndicator _peppiness;

        // protected override void OnEnabled() =>
        //     _peppiness.ProgressBar.Full += MoveToIdleState;
        //
        // protected override void OnDisabled() =>
        //     _peppiness.ProgressBar.Full -= MoveToIdleState;

        protected override void Run()
        {
            if (_peppiness.ProgressBar.Current >= _peppiness.ProgressBar.Max) ;
        }

        private void MoveToIdleState()
        {
            Debug.Log("onFull");
            StateMachine.Enter<AnimalIdleState>();
        }
    }
}