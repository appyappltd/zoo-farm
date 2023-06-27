using Logic.AnimalsBehaviour.Movement;
using StateMachineBase;

namespace Logic.AnimalsStateMachine.Transitions
{
    public class ReachDestinationTransition : Transition
    {
        private readonly NavMeshMover _mover;

        public ReachDestinationTransition( NavMeshMover mover) =>
            _mover = mover;

        public override bool CheckCondition() =>
            _mover.Distance <= _mover.StoppingDistance;
    }
}