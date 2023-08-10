using Logic.Animals.AnimalsBehaviour;
using Logic.Animals.AnimalsBehaviour.AnimalStats;
using Progress;
using StateMachineBase.States;
using UnityEngine;

namespace Code.Logic.Animals.AnimalsStateMachine.States
{
    public abstract class StatChange : PrimeAnimatorState
    {
        private readonly AnimalAnimator _animator;
        private readonly StatIndicator _barIndicator;
        private readonly ProgressBarOperator _barOperator;

        public StatChange(AnimalAnimator animator, StatIndicator barIndicator, float changingSpeed) : base(animator)
        {
            _animator = animator;
            _barIndicator = barIndicator;
            _barOperator = new ProgressBarOperator(barIndicator.ProgressBar, changingSpeed, false);
        }

        protected abstract void PlayAnimation(AnimalAnimator animator);

        protected override void OnEnter()
        {
            _barIndicator.Disable();
            PlayAnimation(_animator);
        }

        protected override void OnExit()
        {
            _barIndicator.Enable();
        }

        protected override void OnUpdate() =>
            _barOperator.Update(Time.deltaTime);
    }
}