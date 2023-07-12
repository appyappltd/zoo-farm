using Logic.Animals.AnimalsBehaviour.Movement;
using Logic.AnimatorStateMachine;
using UnityEngine;

namespace StateMachineBase.States
{
    public class MoveToAndRotate : MoveTo
    {
        private readonly Transform _rotateTo;
        private readonly NavMeshMover _mover;

        public MoveToAndRotate(IPrimeAnimator animator, NavMeshMover mover, Transform target, Transform rotateTo) : base(animator, mover, target)
        {
            _rotateTo = rotateTo;
            _mover = mover;
        }

        protected override void OnExit()
        {
            _mover.RotateTo(_rotateTo.position);
            base.OnExit();
        }
    }
}
