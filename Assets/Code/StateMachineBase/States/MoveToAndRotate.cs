using Logic.AnimatorStateMachine;
using Logic.Movement;
using UnityEngine;

namespace StateMachineBase.States
{
    public class MoveToAndRotate : MoveTo
    {
        private readonly NavMeshMover _mover;

        public MoveToAndRotate(IPrimeAnimator animator, NavMeshMover mover, Transform target) : base(animator, mover, target)
        {
            _mover = mover;
        }

        protected override void OnExit()
        {
            base.OnExit();
        }
    }
}
