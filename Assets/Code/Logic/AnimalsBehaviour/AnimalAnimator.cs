using UnityEngine;

namespace Logic.AnimalsBehaviour
{
    public class AnimalAnimator : MonoBehaviour
    {
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Move = Animator.StringToHash("Move");
        private static readonly int Rest = Animator.StringToHash("Rest");
        private static readonly int Eat = Animator.StringToHash("Eat");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Jump = Animator.StringToHash("Jump");

        [SerializeField] private Animator _animator;

        public void SetIdle() =>
            _animator.CrossFade(Idle, 0.1f);

        public void SetMove() =>
            _animator.CrossFade(Move, 0.1f);

        public void SetRest() =>
            _animator.CrossFade(Rest, 0.1f);

        public void SetEat() =>
            _animator.CrossFade(Eat, 0.1f);

        public void SetJump() =>
            _animator.CrossFade(Jump, 0.1f);

        public void SetSpeed(float speed) =>
            _animator.SetFloat(Speed, speed);
    }
}