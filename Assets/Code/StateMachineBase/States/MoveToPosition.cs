using Logic.AnimatorStateMachine;
using Logic.Movement;
using UnityEngine;

namespace StateMachineBase.States
{
    public class MoveToPosition : Move
    {
        private Vector3 _position;

        public MoveToPosition(IPrimeAnimator animator, NavMeshMover mover, Vector3 target) : base(animator, mover) =>
            _position = target;

        public void SetNewPosition(Vector3 newPosition) =>
            _position = newPosition;

        protected override Vector3 GetMovePosition() =>
            _position;
    }
}