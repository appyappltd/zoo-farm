using Logic.AnimatorStateMachine;
using Logic.Movement;
using UnityEngine;

namespace StateMachineBase.States
{
    public abstract class Move : PrimeAnimatorState
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

        protected override void OnExit()
        {
            _mover.enabled = false;
        }

        protected override void OnUpdate() =>
            Animator.SetSpeed(_mover.NormalizedSpeed);
    }
}