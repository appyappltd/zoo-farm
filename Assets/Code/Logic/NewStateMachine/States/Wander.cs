using Logic.AnimalsBehaviour;
using Logic.AnimalsBehaviour.Movement;
using UnityEngine;

namespace Logic.NewStateMachine.States
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
            _mover.transform.position + new Vector3(
                Random.Range(-_maxDistance, _maxDistance),
                0,
                Random.Range(-_maxDistance, _maxDistance));
    }
}