using Logic.AnimalsBehaviour;
using StateMachineBase;

namespace Logic.AnimalsStateMachine.States
{
    public class AnimalState : State
    {
        protected readonly IPrimeAnimator Animator;
        
        protected AnimalState(IPrimeAnimator animator) =>
            Animator = animator;
    }
}