using Logic.AnimalsBehaviour;

namespace Logic.AnimalsStateMachine.States
{
    public class Idle : AnimalState
    {
        public Idle(AnimalAnimator animator) : base(animator) { }
        protected override void OnEnter() => Animator.SetIdle();
    }
}