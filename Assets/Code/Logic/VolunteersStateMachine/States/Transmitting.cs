using Logic.AnimatorStateMachine;
using StateMachineBase.States;

namespace Logic.VolunteersStateMachine.States
{
    public class Transmitting : Idle
    {
        private readonly Volunteers.Volunteer _volunteer;

        public Transmitting(IPrimeAnimator animator, Volunteers.Volunteer volunteer) : base(animator)
        {
            _volunteer = volunteer;
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            _volunteer.ReadyTransmitting();
        }
    }
}
