using NTC.Global.Cache;

namespace DelayRoutines
{
    public sealed class ConstTimeAwaiter : TimeAwaiter
    {
        private readonly float _waitTime;

        public ConstTimeAwaiter(float waitTime, GlobalUpdate globalUpdate) : base(globalUpdate) =>
            _waitTime = waitTime;

        protected override float GetWaitTime() =>
            _waitTime;
    }
}