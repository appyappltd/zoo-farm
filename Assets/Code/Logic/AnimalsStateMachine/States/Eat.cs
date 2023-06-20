using Logic.AnimalsBehaviour;
using Logic.AnimalsBehaviour.AnimalStats;
using Progress;
using UnityEngine;

namespace Logic.AnimalsStateMachine.States
{
    public class Eat : StatChange
    {
        private readonly ProgressBarOperator _bowlBarOperator;

        public Eat(AnimalAnimator animator, ProgressBarIndicator barIndicator, float changingSpeed, IProgressBar bowlBar) : base(animator, barIndicator, changingSpeed)
        {
            _bowlBarOperator = new ProgressBarOperator(bowlBar, changingSpeed, true);
        }

        protected override void PlayAnimation(AnimalAnimator animator) =>
            animator.SetEat();

        // protected override void OnExit()
        // {
        //     _
        // }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            _bowlBarOperator.Update(Time.deltaTime);
        }
    }
}