using Logic.Movement;
using UnityEngine;

namespace Logic.Volunteer
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AnimationMover))]
    public class VolunteerMovement : MonoBehaviour
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        
        private Animator animC;
        private AnimationMover mover;

        private void Awake()
        {
            animC = GetComponent<Animator>();
            mover = GetComponent<AnimationMover>();
            mover.StartMove += OnMove;
            mover.GotToPlace += OnMove;
        }

        private void OnDestroy()
        {
            mover.StartMove -= OnMove;
            mover.GotToPlace -= OnMove;
        }

        private void OnMove() =>
            animC.SetBool(IsMoving, mover.IsMoving);
    }
}
