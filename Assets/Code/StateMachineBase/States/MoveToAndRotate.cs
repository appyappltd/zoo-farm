using Logic.AnimatorStateMachine;
using Logic.Movement;
using UnityEngine;

namespace StateMachineBase.States
{
    public class MoveToAndRotate : MoveTo
    {
        private readonly Aligner _aligner;

        public MoveToAndRotate(IPrimeAnimator animator, NavMeshMover mover, Transform target, Aligner aligner) : base(animator, mover, target)
        {
            _aligner = aligner;
        }

        protected override void OnExit()
        {
            _aligner.Aligne(Target.rotation);
            base.OnExit();
        }
    }
}
