using Logic.AnimatorStateMachine;
using Logic.Movement;
using UnityEngine;

namespace StateMachineBase.States
{
    public class LookAt : PrimeAnimatorState
    {
        private readonly NavMeshMover _mover;
        private readonly Transform _lookAt;

        protected LookAt(IPrimeAnimator animator, NavMeshMover mover, Transform lookAt) : base(animator)
        {
            _mover = mover;
            _lookAt = lookAt;
        }

        protected override void OnEnter() =>
            _mover.RotateTo(_lookAt.position);
    }
}