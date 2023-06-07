using Logic.AnimalsBehaviour.Movement;
using MonoStateMachine;
using Tools.Extension;
using UnityEngine;

namespace Logic.AnimalsBehaviour.States
{
    public class AnimalMoveState : State
    {
        [SerializeField] private AnimalAnimator _animator;
        [SerializeField] private AnimalMover _mover;
        [SerializeField] private float _maxMoveDistance;

        protected override void OnEnabled()
        {
            _animator.SetMove();

            Vector2 randomOffset = new Vector2(Random.Range(-_maxMoveDistance, _maxMoveDistance),
                Random.Range(-_maxMoveDistance, _maxMoveDistance));

            Vector2 destination = transform.position + randomOffset.AddY(0);
            _mover.SetDestination(destination);
        }

        protected override void OnDisabled() =>
            _mover.enabled = false;
    }
}