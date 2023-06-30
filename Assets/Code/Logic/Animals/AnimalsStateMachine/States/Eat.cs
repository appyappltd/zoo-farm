using Logic.Animals.AnimalsBehaviour;
using Logic.Animals.AnimalsBehaviour.AnimalStats;
using Progress;
using UnityEngine;

namespace Logic.Animals.AnimalsStateMachine.States
{
    public class Eat : StatChange
    {
        private readonly ProgressBarOperator _bowlBarOperator;

        public Eat(AnimalAnimator animator, StatIndicator barIndicator, float changingSpeed,
            IProgressBar bowlBar)
            : base(animator, barIndicator, changingSpeed)
        {
            _bowlBarOperator = new ProgressBarOperator(bowlBar, changingSpeed, true);
        }

        protected override void PlayAnimation(AnimalAnimator animator) =>
            animator.SetEat();

        protected override void OnUpdate()
        {
            base.OnUpdate();
            _bowlBarOperator.Update(Time.deltaTime);
        }
    }
}