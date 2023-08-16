using Logic.AnimatorStateMachine;
using Logic.Movement;
using NTC.Global.System;
using StateMachineBase.States;
using UnityEngine;

namespace Logic.Animals.AnimalsStateMachine
{
    public class ForceMove : MoveTo
    {
        private readonly Aligner _aligner;

        public ForceMove(IPrimeAnimator animator, NavMeshMover mover, Aligner aligner) : base(animator, mover, null) =>
            _aligner = aligner;

        protected override void OnEnter()
        {
            _aligner.Disable();
            base.OnEnter();
        }

        protected override void OnExit()
        {
            _aligner.Aligne(Target.rotation);
            base.OnExit();
        }

        public void SetNewPosition(Transform newTarget) =>
            Target = newTarget;
    }
}