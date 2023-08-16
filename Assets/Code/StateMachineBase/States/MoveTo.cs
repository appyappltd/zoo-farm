using Logic.AnimatorStateMachine;
using Logic.Movement;
using UnityEngine;

namespace StateMachineBase.States
{
    public class MoveTo : Move
    {
        protected Transform Target;

        public MoveTo(IPrimeAnimator animator, NavMeshMover mover, Transform target) : base(animator, mover) =>
            Target = target;

        protected override Vector3 GetMovePosition() =>
            Target.position;
    }
}