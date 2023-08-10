using Code.Logic.Animals.AnimalsStateMachine.States;
using Logic.Animals.AnimalsBehaviour;
using Logic.Animals.AnimalsBehaviour.AnimalStats;
using Progress;
using UnityEngine;

namespace Logic.Animals.AnimalsStateMachine.States
{
    public class ForceEat : StatChange
    {
        private readonly StatIndicator _barIndicator;
        private readonly ProgressBarOperator _bowlBarOperator;

        public ForceEat(AnimalAnimator animator, StatIndicator barIndicator, float changingSpeed, IProgressBar bowlBar)
            : base(animator, barIndicator, changingSpeed)
        {
            _barIndicator = barIndicator;
            _bowlBarOperator = new ProgressBarOperator(bowlBar, changingSpeed, true);
        }

        protected override void PlayAnimation(AnimalAnimator animator) =>
            animator.SetEat();

        protected override void OnEnter()
        {
            _barIndicator.ProgressBar.Reset();
            base.OnEnter();
        }

        protected override void OnExit()
        {
            _barIndicator.ProgressBar.Reset();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            _bowlBarOperator.Update(Time.deltaTime);
        }
    }
}