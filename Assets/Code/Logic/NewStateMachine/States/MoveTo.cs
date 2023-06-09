using Logic.AnimalsBehaviour;
using Logic.AnimalsBehaviour.Movement;
using UnityEngine;

namespace Logic.NewStateMachine.States
{
    public class MoveTo : Move
    {
        private readonly Transform _target;

        public MoveTo(AnimalAnimator animator, AnimalMover mover, Transform target) : base(animator, mover) =>
            _target = target;

        protected override Vector3 GetMovePosition() =>
            _target.position;
    }
}