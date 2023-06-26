using Logic.Animals.AnimalsBehaviour;
using Logic.Animals.AnimalsBehaviour.Movement;
using UnityEngine;

namespace Logic.Animals.AnimalsStateMachine.States
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