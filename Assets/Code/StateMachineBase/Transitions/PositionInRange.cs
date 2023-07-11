using UnityEngine;

namespace StateMachineBase.Transitions
{
    public class PositionInRange : DistanceTo
    {
        private readonly Transform _origin;
        private readonly Transform _target;
        private readonly float _range;
        private Vector3 _targetPos;

        protected override float Distance =>
            Vector3.Distance(_origin.position, _targetPos);

        public PositionInRange(Transform origin, Transform target, float range) : base(origin, target)
        {
            _origin = origin;
            _target = target;
            _range = range;
        }

        public override void Enter()
        {
            _targetPos = _target.position;
        }

        public override bool CheckCondition() =>
            Distance < _range;
    }
}