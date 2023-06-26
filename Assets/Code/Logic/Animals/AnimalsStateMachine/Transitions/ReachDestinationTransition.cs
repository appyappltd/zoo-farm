using Logic.Animals.AnimalsBehaviour.Movement;
using StateMachineBase;

namespace Logic.Animals.AnimalsStateMachine.Transitions
{
    public class ReachDestinationTransition : Transition
    {
        private readonly AnimalMover _mover;

        public ReachDestinationTransition( AnimalMover mover) =>
            _mover = mover;

        public override bool CheckCondition() =>
            _mover.Distance <= _mover.StoppingDistance;
    }
}