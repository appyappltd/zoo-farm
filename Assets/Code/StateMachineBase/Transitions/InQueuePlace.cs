using Logic.NPC.Volunteers;
using UnityEngine;

namespace StateMachineBase.Transitions
{
    public class InQueuePlace : TargetInRange
    {
        private readonly Transform _origin;
        private readonly Volunteer _volunteer;

        private Vector3 _queuePosition;

        protected override float Distance => Vector3.Distance(_origin.position, _queuePosition);

        public InQueuePlace(Transform origin, Transform target, Volunteer volunteer, float range) : base(origin, target, range)
        {
            _origin = origin;
            _volunteer = volunteer;
        }

        public override void Enter()
        {
            base.Enter();
            _queuePosition = _volunteer.QueuePosition;
        }
    }
}