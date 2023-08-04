using System;

namespace StateMachineBase.Transitions
{
    public class CheckTransition : Transition
    {
        private readonly Func<bool> _func;
        private readonly bool _isCheck;

        public CheckTransition(ref Func<bool> func) =>
            _func = func;

        public override bool CheckCondition() =>
            _func.Invoke();
    }
}