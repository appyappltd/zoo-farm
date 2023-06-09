namespace Logic.NewStateMachine.Transitions
{
    public class RandomTimerTransition : TimerTransition
    {
        private readonly float _maxTime;
        private readonly float _minTime;

        public RandomTimerTransition(float maxTime, float minTime) : base(maxTime)
        {
            _maxTime = maxTime;
            _minTime = minTime;
        }

        protected override float GetDelay() =>
            UnityEngine.Random.Range(_minTime, _maxTime);
    }
}