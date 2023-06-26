using Logic.Animals.AnimalsBehaviour;
using Logic.Animals.AnimalsBehaviour.Movement;
using UnityEngine;

namespace Logic.Animals.AnimalsStateMachine.States
{
    public abstract class Move : AnimalState
    {
        private readonly AnimalAnimator _animator;
        private readonly AnimalMover _mover;

        public Move(AnimalAnimator animator, AnimalMover mover) : base(animator)
        {
            _animator = animator;
            _mover = mover;
        }

        protected abstract Vector3 GetMovePosition();

        protected override void OnEnter()
        {
            _animator.SetMove();
            Vector3 movePosition = GetMovePosition();
            _mover.SetDestination(movePosition);
            _mover.enabled = true;
        }

        protected override void OnExit() =>
            _mover.enabled = false;

        protected override void OnUpdate() =>
            Animator.SetSpeed(_mover.NormalizedSpeed);
    }
}