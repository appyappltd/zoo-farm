using System;
using Logic.Animals.AnimalsBehaviour.AnimalStats;
using Logic.AnimatorStateMachine;
using StateMachineBase.States;

namespace Logic.Animals.AnimalsStateMachine.States
{
    public class BreedingIdle : Idle
    {
        private readonly StatIndicator _satiety;

        private Action _onBreedingComplete;
        private Action _onBreedingBegin;

        public BreedingIdle(IPrimeAnimator animator, StatIndicator satiety) : base(animator)
        {
            _satiety = satiety;
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            _onBreedingBegin.Invoke();
        }

        protected override void OnExit()
        {
            _onBreedingComplete.Invoke();
            _satiety.ProgressBar.Reset();
        }

        public void Init(Action onBreedingBegin, Action onBreedingComplete)
        {
            _onBreedingComplete = onBreedingComplete;
            _onBreedingBegin = onBreedingBegin;
        }
    }
}