using NTC.Global.Cache;
using UnityEngine;
using Random = UnityEngine.Random;


namespace DelayRoutines
{
    public sealed class RandomTimeAwaiter : TimeAwaiter
    {
        private readonly Vector2 _timeRange;

        public RandomTimeAwaiter(Vector2 timeRange, GlobalUpdate globalUpdate) : base(globalUpdate) =>
            _timeRange = timeRange;

        protected override float GetWaitTime() =>
            Random.Range(_timeRange.x, _timeRange.y);
    }
}