using Logic.AnimatorStateMachine;
using StateMachineBase.States;
using Unity.VisualScripting;

namespace Logic.VolunteersStateMachine.States
{
    public class Transmitting : Idle
    {
        private Volunteer.Volunteer volunteer;

        protected override void OnEnter()
        {
            base.OnEnter();
            volunteer.CanGiveAnimal = true;
        }

        public Transmitting(IPrimeAnimator animator, Volunteer.Volunteer volunteer) : base(animator)
        {
            this.volunteer = volunteer;
        }
        protected override void OnExit()
        {
            base.OnExit();
            volunteer.CanGiveAnimal = false;
        }
    }
}
