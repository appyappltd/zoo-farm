using System;
using Logic.AnimatorStateMachine;
using StateMachineBase.States;

namespace Logic.Animals.AnimalsStateMachine.States
{
    public class BreedingIdle : Idle
    {
        private Action _onBreedingComplete;
        private Action _onBreedingBegin;

        public BreedingIdle(IPrimeAnimator animator) : base(animator) { }

        protected override void OnEnter()
        {
            base.OnEnter();
            _onBreedingBegin.Invoke();
        }

        protected override void OnExit()
        {
            _onBreedingComplete.Invoke();
        }

        public void Init(Action onBreedingBegin, Action onBreedingComplete)
        {
            _onBreedingComplete = onBreedingComplete;
            _onBreedingBegin = onBreedingBegin;
        }
    }
}