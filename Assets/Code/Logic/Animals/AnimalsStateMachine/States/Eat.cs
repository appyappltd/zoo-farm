using System;
using DelayRoutines;
using Logic.Animals.AnimalFeeders;
using Logic.Animals.AnimalsBehaviour;
using Logic.Animals.AnimalsBehaviour.AnimalStats;
using Progress;
using StateMachineBase;
using UnityEngine;

namespace Logic.Animals.AnimalsStateMachine.States
{
    public class Eat : StatChange, IDisposable
    {
        private readonly StatIndicator _barIndicator;
        private readonly AnimalFeeder _feeder;
        private readonly RoutineSequence _routineSequence = new RoutineSequence();
        private readonly float _changingSpeed;
        
        private ProgressBarOperator _bowlBarOperator;
        private Bowl _bowl;

        public Eat(AnimalAnimator animator, StatIndicator barIndicator, float changingSpeed,
            float hungerDelay, AnimalFeeder feeder)
            : base(animator, barIndicator, changingSpeed)
        {
            _feeder = feeder;
            _changingSpeed = changingSpeed;
            _barIndicator = barIndicator;
            _routineSequence.WaitForSeconds(hungerDelay).Then(() => barIndicator.enabled = true);
        }

        protected override void PlayAnimation(AnimalAnimator animator) =>
            animator.SetEat();

        protected override void OnEnter()
        {
            _barIndicator.ProgressBar.Reset();
            _bowlBarOperator = new ProgressBarOperator(_bowl.ProgressBarView, _changingSpeed, true);
            base.OnEnter();
        }

        protected override void OnExit()
        {
            _routineSequence.Play();
            _feeder.VacateBowl(_bowl);
            _bowl = null;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            _bowlBarOperator.Update(Time.deltaTime);
        }

        public void Dispose()
        {
            _routineSequence.Kill();
        }
        
        public void ApplyBowl(Bowl bowl) =>
            _bowl = bowl ? bowl : throw new NullReferenceException(nameof(bowl));
    }
}