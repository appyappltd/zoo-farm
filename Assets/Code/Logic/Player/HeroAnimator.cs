using UnityEngine;

namespace Logic.Player
{
    public class HeroAnimator : MonoBehaviour
    {
        private static readonly int IsMoving = Animator.StringToHash("Moving");

        [SerializeField] private Animator _animator;

        public void SetMove(bool isMoving)
        {
            _animator.SetBool(IsMoving, isMoving);
        }

        public void SetSpeed(float speed)
        {
            _animator.speed = speed;
        }
    }
}