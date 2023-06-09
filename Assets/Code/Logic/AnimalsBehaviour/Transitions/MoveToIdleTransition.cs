using Logic.AnimalsBehaviour.Movement;
using Logic.AnimalsBehaviour.States;
using MonoStateMachine;
using UnityEngine;

namespace Logic.AnimalsBehaviour.Transitions
{
    public class MoveToIdleTransition : Transition
    {
        [SerializeField] private AnimalMover _mover;

        protected override void Run()
        {
            if (_mover.Distance <= _mover.StoppingDistance)
            {
                StateMachine.Enter<AnimalIdleState>();
            }
        }
    }
}