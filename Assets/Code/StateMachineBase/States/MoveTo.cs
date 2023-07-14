using Logic.AnimatorStateMachine;
using Logic.Movement;
using UnityEngine;

namespace StateMachineBase.States
{
    public class MoveTo : Move
    {
        private readonly Transform _target;

        public MoveTo(IPrimeAnimator animator, NavMeshMover mover, Transform target) : base(animator, mover) =>
            _target = target;

        protected override Vector3 GetMovePosition() =>
            _target.position;
    }
}