using Logic.AnimatorStateMachine;

namespace StateMachineBase.States
{
    public class Idle : PrimeAnimatorState
    {
        public Idle(IPrimeAnimator animator) : base(animator) { } 
        protected override void OnEnter() => Animator.SetIdle();
    }

    public class Waiting : Idle
    {
        public Waiting(IPrimeAnimator animator) : base(animator)
        {
        }
    }
}