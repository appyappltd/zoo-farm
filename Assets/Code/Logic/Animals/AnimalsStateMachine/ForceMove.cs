using Logic.AnimatorStateMachine;
using Logic.Movement;
using StateMachineBase.States;
using UnityEngine;

namespace Logic.Animals.AnimalsStateMachine
{
    public class ForceMove : Move
    {
        private readonly Aligner _aligner;
        private Transform _target;

        public ForceMove(IPrimeAnimator animator, NavMeshMover mover, Aligner aligner) : base(animator, mover)
        {
            _aligner = aligner;
        }

        protected override void OnEnter()
        {
            Debug.Log("Force Move");
            base.OnEnter();
        }

        protected override void OnExit()
        {
            _aligner.Aligne(_target.rotation);
            base.OnExit();
        }

        protected override Vector3 GetMovePosition()
        {
            return _target.position;
        }

        public void SetNewPosition(Transform newTarget) =>
            _target = newTarget;
    }
}