using Logic.Animals.AnimalsBehaviour;
using StateMachineBase;

namespace Logic.Animals.AnimalsStateMachine.States
{
    public class AnimalState : State
    {
        protected readonly AnimalAnimator Animator;
        
        protected AnimalState(AnimalAnimator animator) =>
            Animator = animator;
    }
}