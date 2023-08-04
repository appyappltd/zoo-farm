using Logic.AnimatorStateMachine;
using StateMachineBase.States;

namespace Logic.NPC.Volunteers.VolunteersStateMachine.States
{
    public class Reload : Idle
    {
        private readonly Volunteer _volunteer;
        
        public Reload(IPrimeAnimator animator, Volunteer volunteer) : base(animator)
        {
            _volunteer = volunteer;
        }
        protected override void OnEnter()
        {
            base.OnEnter();
            _volunteer.Reload();
        }
    }
}
