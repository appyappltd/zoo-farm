using Progress;
using StateMachineBase;

namespace Logic.Animals.AnimalsStateMachine.Transitions
{
    public class BarEmptyTransition : Transition
    {
        private readonly IProgressBarView _barView;

        public BarEmptyTransition(IProgressBarView barView)
        {
            _barView = barView;
        }

        public override bool CheckCondition() =>
            _barView.IsEmpty;
    }
}