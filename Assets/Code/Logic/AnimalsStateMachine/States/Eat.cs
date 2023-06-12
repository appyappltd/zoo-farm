using Logic.AnimalsBehaviour;
using Logic.AnimalsBehaviour.AnimalStats;

namespace Logic.AnimalsStateMachine.States
{
    public class Eat : StatChange
    {
        public Eat(AnimalAnimator animator, ProgressBarIndicator barIndicator, float changingSpeed) : base(animator, barIndicator, changingSpeed) { }

        protected override void PlayAnimation(AnimalAnimator animator) =>
            animator.SetEat();
    }
}