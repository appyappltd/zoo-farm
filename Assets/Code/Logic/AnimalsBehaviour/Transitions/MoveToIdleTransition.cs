using Logic.AnimalsBehaviour.Movement;
using Logic.AnimalsBehaviour.States;
using MonoStateMachine;
using UnityEngine;

namespace Logic.AnimalsBehaviour.Transitions
{
    public class MoveToIdleTransition : Transition
    {
        [SerializeField] private AnimalMover _mover;
        [SerializeField] private float _destinationOffset = 0.1f;

        private Transform _selfTransform;

        private void Awake() =>
            _selfTransform = _mover.transform;

        protected override void Run()
        {
            float distance = Vector3.Distance(_selfTransform.position, _mover.DestinationPoint);

            if (_mover.Distance <= _mover.StoppingDistance)
            {
                StateMachine.ChangeState<AnimalIdleState>();
            }
        }
    }
}