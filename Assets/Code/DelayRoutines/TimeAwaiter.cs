using NTC.Global.Cache;
using UnityEngine;

namespace DelayRoutines
{
    public sealed class TimeAwaiter : Awaiter
    {
        private readonly float _waitTime;
        private float _elapsedTime;

        public TimeAwaiter(float waitTime, GlobalUpdate globalUpdate) : base(globalUpdate)
        {
            _waitTime = waitTime;
        }

        protected override void OnPlay()
        {
            _elapsedTime = _waitTime;
            base.OnPlay();
        }

        public override void OnRun()
        {
            _elapsedTime -= Time.deltaTime;

            if (_elapsedTime <= 0)
            {
                Deactivate();
                Next();
            }
        }
    }
}