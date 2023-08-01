using System;

namespace Tools.DelayRoutine
{
    public abstract class Routine : IRoutine
    {
        private Action _executedCallback;

        protected Routine()
        {
            _executedCallback = () => { };
        }

        public virtual void Play() { }

        public void AddNext(IRoutine routine) =>
            _executedCallback = routine.Play;

        protected void Next() =>
            _executedCallback.Invoke();
    }
}