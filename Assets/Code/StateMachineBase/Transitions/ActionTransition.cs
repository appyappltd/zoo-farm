using System;

namespace StateMachineBase.Transitions
{
    public class ActionTransition : Transition, IDisposable
    {
        protected bool IsCondition;

        private Action _unsubscribe;

        public override bool CheckCondition() =>
            IsCondition;

        public override void Exit()
        {
            IsCondition = false;
        }

        public void SetConditionTrue()
        {
            IsCondition = true;
        }

        public void SetUnsubscribeAction(Action action) =>
            _unsubscribe = action;

        public void Dispose() =>
            _unsubscribe.Invoke();
    }
}