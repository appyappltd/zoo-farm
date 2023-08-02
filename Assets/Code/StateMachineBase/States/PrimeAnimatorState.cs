using Logic.AnimatorStateMachine;

namespace StateMachineBase.States
{
    public class PrimeAnimatorState : State
    {
        protected readonly IPrimeAnimator Animator;

        protected PrimeAnimatorState(IPrimeAnimator animator) =>
            Animator = animator;
    }
}