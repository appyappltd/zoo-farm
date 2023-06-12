using System;

namespace StateMachineBase
{
    public abstract class Transition
    {
        public Action Callback;

        public abstract bool CheckCondition();

        public virtual void Enter() { }
        public virtual void Exit() { }

        public void Update()
        {
            if (CheckCondition() == false)
                return;

            if (Callback == null)
            {
                Enter();
                return;
            }

            Callback.Invoke();
        }
    }
}