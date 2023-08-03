using System;
using NTC.Global.Cache;

namespace DelayRoutines
{
    public abstract class ActionAwaiter : Awaiter
    {
        private readonly Func<bool> _waitFunc;

        protected ActionAwaiter(Func<bool> waitFunc, GlobalUpdate globalUpdate) : base(globalUpdate) =>
            _waitFunc = waitFunc;

        protected abstract bool IsAwaiting(Func<bool> waitFunc);

        public override void OnRun()
        {
            if (IsAwaiting(_waitFunc))
                return;

            Deactivate();
            Next();
        }
    }
}