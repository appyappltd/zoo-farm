using Logic.Animals.AnimalFeeders;
using Progress;
using StateMachineBase;

namespace Logic.Animals.AnimalsStateMachine.Transitions
{
    public class EmptySatietyAndHasFreeBowl : Transition
    {
        private readonly IProgressBar _satiety;
        private readonly AnimalFeeder _feeder;
        
        private bool _isEmptySatiety;

        public EmptySatietyAndHasFreeBowl(IProgressBar satiety, AnimalFeeder feeder)
        {
            _satiety = satiety;
            _feeder = feeder;
        }

        public override bool CheckCondition()
        {
            return _isEmptySatiety && HasFreeBowl();
        }

        public override void Enter()
        {
            _isEmptySatiety = _satiety.IsEmpty;
        }

        public override void Exit()
        {
            _isEmptySatiety = false;
        }

        private bool HasFreeBowl() =>
            _feeder.TryReserveBowl();
    }
}