using UnityEngine;

namespace StateMachineBase.Transitions
{
    public class TargetInRange : DistanceTo
    {
        private readonly float _range;
        private readonly Transform _origin;
        private readonly Transform _target;
        
        private Vector3 _targetPos;

        public TargetInRange(Transform origin, Transform target, float range) : base(origin, target)
        {
            _origin = origin;
            _target = target;
            _range = range;
        }

        public override void Enter()
        {
            base.Enter();
            _targetPos = _target.position;
        }
        
        public override bool CheckCondition()
        {
            Debug.Log(Vector3.Distance(_origin.position, _targetPos));
            return Vector3.Distance(_origin.position, _targetPos) < _range;
        }
    }
}
