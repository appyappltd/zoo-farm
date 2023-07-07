using Logic.Movement;
using UnityEngine;

namespace Logic.Volunteers
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AnimationItemMover))]
    public class VolunteerMovement : MonoBehaviour
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        
        private Animator animC;
        private AnimationItemMover _itemMover;

        private void Awake()
        {
            animC = GetComponent<Animator>();
            _itemMover = GetComponent<AnimationItemMover>();
            _itemMover.Started += OnMove;
            _itemMover.Ended += OnMove;
        }

        private void OnDestroy()
        {
            _itemMover.Started -= OnMove;
            _itemMover.Ended -= OnMove;
        }

        private void OnMove() =>
            animC.SetBool(IsMoving, _itemMover.IsMoving);
    }
}
