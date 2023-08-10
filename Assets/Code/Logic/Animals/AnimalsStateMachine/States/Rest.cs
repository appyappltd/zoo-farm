using Logic.Animals.AnimalsBehaviour;
using Logic.Animals.AnimalsBehaviour.AnimalStats;

namespace Code.Logic.Animals.AnimalsStateMachine.States
{
    public class Rest : StatChange
    {
        public Rest(AnimalAnimator animator, StatIndicator barIndicator, float changingSpeed) : base(animator, barIndicator, changingSpeed) { }
        protected override void PlayAnimation(AnimalAnimator animator) => animator.SetRest();
    }
}