using Logic.Volunteers;
using UnityEngine;

namespace StateMachineBase.Transitions
{
    public class OutOfQueuePlace : TargetOutOfRange
    {
        private readonly Transform _origin;
        private readonly Volunteer _volunteer;

        protected override float Distance => Vector3.Distance(_origin.position, _volunteer.QueuePosition);
        
        public OutOfQueuePlace(Transform origin, Transform target, Volunteer volunteer, float range) : base(origin, target, range)
        {
            _origin = origin;
            _volunteer = volunteer;
        }
    }
}