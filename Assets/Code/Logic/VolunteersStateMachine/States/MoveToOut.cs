using Logic.Animals.AnimalsBehaviour.Movement;
using Logic.AnimatorStateMachine;
using StateMachineBase.States;
using UnityEngine;

namespace Logic.VolunteersStateMachine.States
{
    public class MoveToOut : MoveTo
    {
        private Volunteer.Volunteer volunteer;
        public MoveToOut(IPrimeAnimator animator, NavMeshMover mover, Transform target, Volunteer.Volunteer volunteer) : base(animator, mover, target)
        {
            this.volunteer = volunteer;
        }
        protected override void OnEnter()
        {
            base.OnEnter();
            volunteer.CanGiveAnimal = false;
        }
    }
}
