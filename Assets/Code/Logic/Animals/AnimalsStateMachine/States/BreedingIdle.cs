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

        public BreedingIdle(IPrimeAnimator animator, StatIndicator satiety) : base(animator)
        {
            _satiety = satiety;
        }

        protected override void OnExit()
        {
            _onBreedingComplete.Invoke();
            _satiety.ProgressBar.Reset();
        }

        public void Init(Action onBreedingComplete)
        {
            _onBreedingComplete = onBreedingComplete;
        }
    }
}