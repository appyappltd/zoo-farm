using System;
using DelayRoutines;
using Logic.Animals.AnimalsBehaviour;
using Logic.Animals.AnimalsBehaviour.AnimalStats;
using Progress;
using UnityEngine;

namespace Logic.Animals.AnimalsStateMachine.States
{
    public class Eat : StatChange, IDisposable
    {
        private readonly StatIndicator _barIndicator;
        private readonly ProgressBarOperator _bowlBarOperator;
        private readonly DelayRoutine _routine = new DelayRoutine();

        public Eat(AnimalAnimator animator, StatIndicator barIndicator, float changingSpeed, float hungerDelay, IProgressBar bowlBar)
            : base(animator, barIndicator, changingSpeed)
        {
            _barIndicator = barIndicator;
            _bowlBarOperator = new ProgressBarOperator(bowlBar, changingSpeed, true);
            _routine.WaitForSeconds(hungerDelay).Then(() => barIndicator.enabled = true);
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
            _routine.Play();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            _bowlBarOperator.Update(Time.deltaTime);
        }

        public void Dispose()
        {
            _routine.Kill();
        }
    }
}