using System;
using StateMachineBase;

namespace Logic.NewStateMachine.Transitions
{
    public class CheckTransition : Transition
    {
        private readonly Func<bool> _func;
        private readonly bool _isCheck;

        public CheckTransition(Func<bool> func) =>
            _func = func;

        public override bool CheckCondition() =>
            _func.Invoke();
    }
}