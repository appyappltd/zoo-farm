using Logic.AnimalsBehaviour;
using Logic.AnimalsBehaviour.AnimalStats;

namespace Logic.NewStateMachine.States
{
    public class Rest : StatChange
    {
        public Rest(AnimalAnimator animator, ProgressBarIndicator barIndicator, float changingSpeed) : base(animator, barIndicator, changingSpeed) { }
        protected override void PlayAnimation(AnimalAnimator animator) => animator.SetRest();
    }
}