using Logic.AnimatorStateMachine;
using StateMachineBase.States;

namespace Logic.VolunteersStateMachine.States
{
    public class Transmitting : Idle
    {
        private Volunteers.Volunteer volunteer;

        public Transmitting(IPrimeAnimator animator, Volunteers.Volunteer volunteer) : base(animator)
        {
            this.volunteer = volunteer;
        }
        
    }
}
