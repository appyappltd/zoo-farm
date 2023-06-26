using StateMachineBase;
using UnityEngine;

namespace Logic.Animals.AnimalsStateMachine.Transitions
{
    public abstract class DistanceTo : Transition
    {
        private readonly Transform _origin;
        private readonly Transform _target;

        protected float Distance => Vector3.Distance(_origin.position, _target.position);

        protected DistanceTo(Transform origin, Transform target)
        {
            _origin = origin;
            _target = target;
        }
    }
}