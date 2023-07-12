using UnityEngine;

namespace StateMachineBase.Transitions
{
    public class PositionInRange : DistanceTo
    {
        private readonly Transform _origin;
        private readonly float _range;
        private readonly Vector3 _targetPos;

        protected override float Distance =>
            Vector3.Distance(_origin.position, _targetPos);

        public PositionInRange(Transform origin, Transform target, float range) : base(origin, target)
        {
            _origin = origin;
            _range = range;
            _targetPos = target.position;
        }

        public override bool CheckCondition() =>
            Distance < _range;
    }
}