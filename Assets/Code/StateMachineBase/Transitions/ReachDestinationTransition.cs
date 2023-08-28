using Logic.Movement;

namespace StateMachineBase.Transitions
{
    public class ReachDestinationTransition : Transition
    {
        private readonly NavMeshMover _mover;
        private readonly float _stoppingDistance;

        public ReachDestinationTransition(NavMeshMover mover)
        {
            _mover = mover;
            _stoppingDistance = _mover.StoppingDistance;
        }
        
        public ReachDestinationTransition(NavMeshMover mover, float stoppingDistance)
        {
            _mover = mover;
            _stoppingDistance = stoppingDistance;
        }

        public override bool CheckCondition() =>
            _mover.Distance <= _stoppingDistance;
        
    }
}