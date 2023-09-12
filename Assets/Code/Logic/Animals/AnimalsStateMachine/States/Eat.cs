using System;
using DelayRoutines;
using Logic.Animals.AnimalFeeders;
using Logic.Animals.AnimalsBehaviour;
using Logic.Animals.AnimalsBehaviour.AnimalStats;
using Logic.Animals.AnimalsBehaviour.Emotions;
using Progress;
using UnityEngine;

namespace Logic.Animals.AnimalsStateMachine.States
{
    public class Eat : StatChange, IDisposable
    {
        private readonly StatIndicator _barIndicator;
        private readonly AnimalFeeder _feeder;
        private readonly PersonalEmotionService _emotionService;
        private readonly RoutineSequence _routineSequence;
        private readonly float _changingSpeed;
        
        private ProgressBarOperator _bowlBarOperator;
        private Bowl _bowl;

        public Eat(AnimalAnimator animator, StatIndicator barIndicator, float changingSpeed,
            float hungerDelay, AnimalFeeder feeder, PersonalEmotionService emotionService)
            : base(animator, barIndicator, changingSpeed)
        {
            _feeder = feeder;
            _emotionService = emotionService;
            _changingSpeed = changingSpeed;
            _barIndicator = barIndicator;
            _routineSequence = new RoutineSequence()
                .WaitForSeconds(hungerDelay)
                .Then(barIndicator.Enable);
        }

        protected override void PlayAnimation(AnimalAnimator animator) =>
            animator.SetEat();

        protected override void OnEnter()
        {
            _barIndicator.ProgressBar.Reset();
            _bowlBarOperator = new ProgressBarOperator(_bowl.ProgressBarView, _changingSpeed, true);
            _emotionService.Show(EmotionId.Eating);
            base.OnEnter();
        }

        protected override void OnExit()
        {
            _routineSequence.Play();
            _feeder.VacateBowl(_bowl);
            _bowl = null;
            _emotionService.Suppress(EmotionId.Eating);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            _bowlBarOperator.Update(Time.deltaTime);
        }

        public void Dispose() =>
            _routineSequence.Kill();

        public void ApplyBowl(Bowl bowl) =>
            _bowl = bowl ? bowl : throw new NullReferenceException(nameof(bowl));
    }
}