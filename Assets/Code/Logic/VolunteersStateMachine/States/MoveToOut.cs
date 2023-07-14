using Logic.AnimatorStateMachine;
using Logic.Movement;
using StateMachineBase.States;
using UnityEngine;

namespace Logic.VolunteersStateMachine.States
{
    public class MoveToOut : MoveTo
    {
        public MoveToOut(IPrimeAnimator animator, NavMeshMover mover, Transform target, Volunteers.Volunteer volunteer) : base(animator, mover, target)
        {
        }
        protected override void OnEnter()
        {
            base.OnEnter();
        }
    }
}
