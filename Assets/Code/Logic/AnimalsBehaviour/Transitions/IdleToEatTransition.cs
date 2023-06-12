using Logic.AnimalsBehaviour.AnimalStats;
using Logic.AnimalsBehaviour.States;
using MonoStateMachine;
using UnityEngine;

namespace Logic.AnimalsBehaviour.Transitions
{
    public class IdleToEatTransition : Transition
    {
        [Header("Component References")]
        [SerializeField] private ProgressBarIndicator _satiety;
        [SerializeField] private Transform _eatPlace;
        
        [Header("Rate Settings")] [Space]
        [SerializeField] private float _restPlaceOffset = 0.5f;

        // protected override void OnEnabled()
        // {
        //     _peppiness.ProgressBar.Empty += MoveToRest;
        // }
        //
        // protected override void OnDisabled()
        // {
        //     _peppiness.ProgressBar.Empty -= MoveToRest;
        // }

        protected override void Run()
        {
            if (_satiety.ProgressBar.IsEmpty)
            {
                MoveToEat();
                Disable();
            }
        }

        private void MoveToEat()
        {
            if (IsInRestPlace())
            {
                StateMachine.Enter<AnimalEatState>();
                
                return;
            }
            
            StateMachine.Enter<AnimalMoveState, Vector3>(_eatPlace.position);
        }
        
        private bool IsInRestPlace() =>
            Vector3.Distance(transform.position, _eatPlace.position) <= _restPlaceOffset;
    }
}