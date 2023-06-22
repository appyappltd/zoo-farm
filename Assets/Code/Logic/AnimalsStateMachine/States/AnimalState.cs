using Logic.AnimalsBehaviour;
using StateMachineBase;

namespace Logic.AnimalsStateMachine.States
{
    public class AnimalState : State
    {
        protected readonly AnimalAnimator Animator;
        
        protected AnimalState(AnimalAnimator animator) =>
            Animator = animator;
    }
}