using UnityEngine;

namespace StateMachineBase.Transitions
{
    public abstract class DistanceTo : Transition
    {
        protected const float DistanceError = 0.05f;
        
        private readonly Transform _origin;
        private readonly Transform _target;
        protected virtual float Distance => Vector3.Distance(_origin.position, _target.position);

        protected DistanceTo(Transform origin, Transform target)
        {
            _origin = origin;
            _target = target;
        }
    }
}