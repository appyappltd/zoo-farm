using Logic.AnimatorStateMachine;
using Logic.Movement;
using UnityEngine;

namespace StateMachineBase.States
{
    public class MoveToPosition : Move
    {
        private readonly Vector3 _position;

        public MoveToPosition(IPrimeAnimator animator, NavMeshMover mover, Transform target) : base(animator, mover) =>
            _position = target.position;

        protected override Vector3 GetMovePosition() =>
            _position;
    }
}