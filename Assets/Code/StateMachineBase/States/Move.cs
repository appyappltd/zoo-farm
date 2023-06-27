using Logic.AnimalsBehaviour;
using Logic.AnimalsBehaviour.Movement;
using UnityEngine;

namespace Logic.AnimalsStateMachine.States
{
    public abstract class Move : AnimalState
    {
        private readonly IPrimeAnimator _animator;
        private readonly NavMeshMover _mover;

        public Move(IPrimeAnimator animator, NavMeshMover mover) : base(animator)
        {
            _animator = animator;
            _mover = mover;
        }

        protected abstract Vector3 GetMovePosition();

        protected override void OnEnter()
        {
            _animator.SetMove();
            Vector3 movePosition = GetMovePosition();
            _mover.SetDestination(movePosition);
            _mover.enabled = true;
        }

        protected override void OnExit() =>
            _mover.enabled = false;

        protected override void OnUpdate() =>
            Animator.SetSpeed(_mover.NormalizedSpeed);
    }
}