using StateMachineBase;
using UnityEngine;

namespace Logic.Animals.AnimalsStateMachine.Transitions
{
    public class TimerTransition : Transition
    {
        private readonly float _maxTime;

        private float _currentTime;

        public TimerTransition(float maxTime) => _maxTime = maxTime;

        protected virtual float GetDelay() =>
            _maxTime;

        public override void Enter() =>
            _currentTime = GetDelay();

        public override bool CheckCondition() =>
            (_currentTime -= Time.deltaTime) <= 0f;

        public float Progress =>
            _currentTime / _maxTime;
    }
}