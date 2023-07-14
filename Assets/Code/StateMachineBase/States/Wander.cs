using Logic.AnimatorStateMachine;
using Logic.Movement;
using Tools.Extension;
using UnityEngine;

namespace StateMachineBase.States
{
    public class Wander : Move
    {
        private readonly NavMeshMover _mover;
        private readonly float _maxDistance;

        public Wander(IPrimeAnimator animator, NavMeshMover mover, float maxDistance) : base(animator, mover)
        {
            _mover = mover;
            _maxDistance = maxDistance;
        }

        protected override Vector3 GetMovePosition() =>
            _mover.transform.position.GetRandomAroundPosition(new Vector3(_maxDistance, 0, _maxDistance));
    }
}