using System;

namespace Logic.AnimalsStateMachine.Transitions
{
    public class ActionPreCheckTransition : ActionTransition
    {
        private readonly Func<bool> _func;

        public ActionPreCheckTransition(Func<bool> func) =>
            _func = func;

        public override void Enter() =>
            IsCondition = _func.Invoke();
    }
}