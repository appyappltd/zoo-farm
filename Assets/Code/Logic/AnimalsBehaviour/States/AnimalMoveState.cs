using Logic.AnimalsBehaviour.Movement;
using MonoStateMachine;
using UnityEngine;

namespace Logic.AnimalsBehaviour.States
{
    public class AnimalMoveState : State
    {
        [SerializeField] private AnimalAnimator _animator;
        [SerializeField] private AnimalMover _mover;
        [SerializeField] private float _maxMoveDistance;

        protected override void Run() =>
            _animator.SetSpeed(_mover.NormalizedSpeed);

        protected override void OnEnabled()
        {
            _animator.SetMove();

            Vector3 randomOffset = new Vector3(
                Random.Range(-_maxMoveDistance, _maxMoveDistance),
                0,
                Random.Range(-_maxMoveDistance, _maxMoveDistance));

            Debug.Log(randomOffset);

            Vector3 destination = transform.position + randomOffset;
            _mover.SetDestination(destination);
            _mover.enabled = true;
        }

        protected override void OnDisabled() =>
            _mover.enabled = false;
    }
}