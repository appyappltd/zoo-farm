using Logic.AnimatorStateMachine;
using Logic.Movement;
using NTC.Global.System;
using StateMachineBase.States;

namespace Logic.NPC.Volunteers.VolunteersStateMachine.States
{
    public class Transmitting : Idle
    {
        private readonly Volunteers.Volunteer _volunteer;
        private readonly Aligner _aligner;

        public Transmitting(IPrimeAnimator animator, Volunteer volunteer, Aligner aligner) : base(animator)
        {
            _volunteer = volunteer;
            _aligner = aligner;
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            _volunteer.ActivateTransmitting();
            _aligner.Aligne(_volunteer.QueueRotation);
        }

        protected override void OnExit()
        {
            _volunteer.DeactivateTransmitting();
            _aligner.Disable();
        }
    }
}
