using Logic.AnimatorStateMachine;
using Logic.Movement;
using UnityEngine;

namespace StateMachineBase.States
{
    public class LookAt : PrimeAnimatorState
    {
        private readonly IPrimeAnimator _animator;
        private readonly Aligner _aligner;
        private readonly Transform _lookAt;

        protected LookAt(IPrimeAnimator animator, Aligner aligner, Transform lookAt) : base(animator)
        {
            _animator = animator;
            _aligner = aligner;
            _lookAt = lookAt;
        }

        protected override void OnEnter()
        {
            _animator.SetIdle();
        }
    }
}