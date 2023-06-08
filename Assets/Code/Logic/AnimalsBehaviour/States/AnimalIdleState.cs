using MonoStateMachine;
using UnityEngine;

namespace Logic.AnimalsBehaviour.States
{
    public class AnimalIdleState : State
    {
        [SerializeField] private AnimalAnimator _animator;
        
        protected override void OnEnter()
        {
            _animator.SetIdle();
        }

        protected override void OnExit()
        {
        }
    }
}