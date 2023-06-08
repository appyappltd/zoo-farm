using System.Collections;
using Logic.AnimalsBehaviour.States;
using MonoStateMachine;
using UnityEngine;

namespace Logic.AnimalsBehaviour.Transitions
{
    public class RestToMoveTransition : Transition
    {
        [SerializeField] private Transform _restPlace;
        [SerializeField] private float _restPlaceOffset = 0.5f;
        
        protected override void OnEnabled()
        {
            StartCoroutine(Delay());
        }

        private IEnumerator Delay()
        {
            yield return null;
            
            if (IsInRestPlace() == false)
            {
                StateMachine.Enter<AnimalMoveState, Vector3>(_restPlace.position);
                Disable();
            }
        }
        
        // protected override void Run()
        // {
        //
        // }

        private bool IsInRestPlace() =>
            Vector3.Distance(transform.position, _restPlace.position) <= _restPlaceOffset;
    }
}