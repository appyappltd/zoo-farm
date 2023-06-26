using UnityEngine;

namespace Logic.Animals.AnimalsStateMachine.Transitions
{
    public class TargetInRange : DistanceTo
    {
        private readonly float _range;

        public TargetInRange(Transform origin, Transform target, float range) : base(origin, target) =>
            _range = range;

        public override bool CheckCondition() => Distance < _range;
    }
}