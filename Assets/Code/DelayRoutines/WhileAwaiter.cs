using System;
using NTC.Global.Cache;

namespace DelayRoutines
{
    public class WhileAwaiter : ActionAwaiter
    {
        public WhileAwaiter(Func<bool> waitFunc, GlobalUpdate globalUpdate)
            : base(waitFunc, globalUpdate) { }

        protected override bool IsAwaiting(Func<bool> waitFunc) =>
            waitFunc.Invoke();
    }
}