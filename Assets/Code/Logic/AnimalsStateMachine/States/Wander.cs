using Logic.AnimalsBehaviour;
using Logic.AnimalsBehaviour.Movement;
using Tools.Extension;
using UnityEngine;

namespace Logic.AnimalsStateMachine.States
{
    public class Wander : Move
    {
        private readonly AnimalMover _mover;
        private readonly float _maxDistance;

        public Wander(AnimalAnimator animator, AnimalMover mover, float maxDistance) : base(animator, mover)
        {
            _mover = mover;
            _maxDistance = maxDistance;
        }

        protected override Vector3 GetMovePosition() =>
            _mover.transform.position.GetRandomAroundPosition(new Vector3(_maxDistance, 0, _maxDistance));
    }
}