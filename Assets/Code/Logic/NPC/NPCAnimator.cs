using Logic.AnimatorStateMachine;
using UnityEngine;

namespace Logic.NPC
{
    public class NPCAnimator : MonoBehaviour, IPrimeAnimator
    {
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Move = Animator.StringToHash("Move");
        private static readonly int Speed = Animator.StringToHash("Speed");

        private Animator _animator;

        private void Awake() =>
            _animator = GetComponent<Animator>();

        public void SetIdle() =>
            _animator.CrossFade(Idle, 0.1f);

        public void SetMove() =>
            _animator.CrossFade(Move, 0.1f);

        public void SetSpeed(float speed) =>
            _animator.SetFloat(Speed, speed);
    }
}
