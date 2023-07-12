using Logic.Volunteers;
using UnityEngine;

namespace StateMachineBase.Transitions
{
    public class InQueuePlace : TargetInRange
    {
        private readonly Transform _origin;
        private readonly Volunteer _volunteer;
        private readonly float _range;
        private Vector3 _place;

        protected override float Distance => Vector3.Distance(_origin.position, _place);

        public InQueuePlace(Transform origin, Transform target, Volunteer volunteer, float range) : base(origin, target, range)
        {
            _origin = origin;
            _volunteer = volunteer;
        }

        public override void Enter()
        {
            _place = _volunteer.QueuePosition;
            base.Enter();
        }
    }
}