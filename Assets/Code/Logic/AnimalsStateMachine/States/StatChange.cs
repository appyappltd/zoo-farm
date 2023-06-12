using Logic.AnimalsBehaviour;
using Logic.AnimalsBehaviour.AnimalStats;
using Progress;
using StateMachineBase;
using UnityEngine;

namespace Logic.AnimalsStateMachine.States
{
    public abstract class StatChange : State
    {
        private readonly AnimalAnimator _animator;
        private readonly ProgressBarIndicator _barIndicator;
        private readonly ProgressBarOperator _barOperator;

        public StatChange(AnimalAnimator animator, ProgressBarIndicator barIndicator, float changingSpeed) : base(animator)
        {
            _animator = animator;
            _barIndicator = barIndicator;
            _barOperator = new ProgressBarOperator(barIndicator.ProgressBar, changingSpeed, false);
        }

        protected abstract void PlayAnimation(AnimalAnimator animator);

        protected override void OnEnter()
        {
            _barIndicator.enabled = false;
            PlayAnimation(_animator);
        }

        protected override void OnExit() =>
            _barIndicator.enabled = true;

        protected override void OnUpdate() =>
            _barOperator.Update(Time.deltaTime);
    }
}