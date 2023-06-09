using Logic.AnimalsBehaviour.States;
using MonoStateMachine;
using UnityEngine;

namespace Logic.AnimalsBehaviour.Transitions
{
    public class IdleToRestTransition : Transition
    {
        [Header("Component References")]
        [SerializeField] private ProgressBarIndicator _peppiness;
        [SerializeField] private Transform _restPlace;
        
        [Header("Rate Settings")] [Space]
        [SerializeField] private float _restPositionOffset;

        protected override void OnEnabled()
        {
            _peppiness.ProgressBar.Empty += MoveToRest;
        }

        protected override void OnDisabled()
        {
            _peppiness.ProgressBar.Empty -= MoveToRest;
        }

        protected override void Run()
        {
            if (_peppiness.ProgressBar.IsEmpty)
            {
                MoveToRest();
            }
        }

        private void MoveToRest()
        {
            StateMachine.Enter<AnimalRestState>();
        }
    }
}