using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Logic.Animals.AnimalsStateMachine.Transitions
{
    public class TargetInRange : DistanceTo
    {
        private readonly float _range;
        private Vector3 targetPos;
        private readonly Transform _origin;
        private readonly Transform _target;

        public TargetInRange(Transform origin, Transform target, float range) : base(origin, target)
        {
            _origin = origin;
            _target = target;
            _range = range;
        }

        public override void Enter()
        {
            base.Enter();
            targetPos = _target.position;
        }
        
        public override bool CheckCondition() => Vector3.Distance(_origin.position, targetPos) < _range;
    }
}