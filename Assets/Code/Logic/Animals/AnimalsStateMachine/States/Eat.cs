using DelayRoutines;
using Logic.Animals.AnimalsBehaviour;
using Logic.Animals.AnimalsBehaviour.AnimalStats;
using Progress;
using UnityEngine;

namespace Logic.Animals.AnimalsStateMachine.States
{
    public class Eat : StatChange
    {
        private readonly StatIndicator _barIndicator;
        private readonly ProgressBarOperator _bowlBarOperator;
        private readonly DelayRoutine _routine = new DelayRoutine();

        public Eat(AnimalAnimator animator, StatIndicator barIndicator, float changingSpeed, float hungerDelay, IProgressBar bowlBar)
            : base(animator, barIndicator, changingSpeed)
        {
            _bowlBarOperator = new ProgressBarOperator(bowlBar, changingSpeed, true);
            _routine.Wait(hungerDelay).Then(() => barIndicator.enabled = true);
        }

        protected override void PlayAnimation(AnimalAnimator animator) =>
            animator.SetEat();

        protected override void OnExit()
        {
            _routine.Play();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            _bowlBarOperator.Update(Time.deltaTime);
        }
    }
}