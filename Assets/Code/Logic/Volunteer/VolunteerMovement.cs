using Logic.Movement;
using UnityEngine;

namespace Logic.Volunteer
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AnimationMover))]
    public class VolunteerMovement : MonoBehaviour
    {
        private Animator animC;
        private AnimationMover mover;

        private void Awake()
        {
            animC = GetComponent<Animator>();
            mover = GetComponent<AnimationMover>();
            mover.StartMove += OnMove;
            mover.GotToPlace += OnMove;
        }

        private void OnMove() => animC.SetBool("IsMoving", mover.IsMoving);
    }
}
