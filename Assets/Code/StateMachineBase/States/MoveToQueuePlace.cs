using Logic.AnimatorStateMachine;
using Logic.Movement;
using Logic.NPC.Volunteers;
using UnityEngine;

namespace StateMachineBase.States
{
    public class MoveToQueuePlace : MoveTo
    {
        private readonly Volunteer _volunteer;
        private readonly Aligner _aligner;

        public MoveToQueuePlace(IPrimeAnimator animator, NavMeshMover mover, Transform target, Volunteer volunteer) : base(animator, mover, target)
        {
            _volunteer = volunteer;
        }

        protected override Vector3 GetMovePosition() =>
            _volunteer.QueuePosition;
    }
}