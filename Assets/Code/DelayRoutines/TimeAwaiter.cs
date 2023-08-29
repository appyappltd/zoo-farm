using NTC.Global.Cache;
using UnityEngine;

namespace DelayRoutines
{
    public abstract class TimeAwaiter : Awaiter
    {
        private float _elapsedTime;

        protected TimeAwaiter(GlobalUpdate globalUpdate) : base(globalUpdate) { }

        protected override void OnPlay()
        {
            _elapsedTime = GetWaitTime();
            base.OnPlay();
        }

        protected abstract float GetWaitTime();

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