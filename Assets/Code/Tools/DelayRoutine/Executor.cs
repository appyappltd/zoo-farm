using System;

namespace Tools.DelayRoutine
{
    public sealed class Executor : Routine
    {
        private readonly Action _action;

        public Executor(Action action) =>
            _action = action;

        public override void Play()
        {
            _action.Invoke();
            Next();
        }
    }
}