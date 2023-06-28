using Logic.Animals.AnimalsBehaviour.Movement;
using Logic.AnimatorStateMachine;
using StateMachineBase.States;
using UnityEngine;

namespace Logic.VolunteersStateMachine.States
{
    public class MoveToOut : MoveTo
    {
        public MoveToOut(IPrimeAnimator animator, NavMeshMover mover, Transform target, Volunteer.Volunteer volunteer) : base(animator, mover, target)
        {
        }
        protected override void OnEnter()
        {
            base.OnEnter();
        }
    }
}
