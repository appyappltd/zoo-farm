using Progress;
using StateMachineBase;

namespace Logic.Animals.AnimalsStateMachine.Transitions
{
    public class BarNotFullTransition : Transition
    {
        private readonly IProgressBarView _barView;

        public BarNotFullTransition(IProgressBarView barView)
        {
            _barView = barView;
        }

        public override bool CheckCondition() =>
            !_barView.IsFull;
    }
}