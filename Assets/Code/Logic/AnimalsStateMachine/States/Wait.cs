using Logic.AnimalsBehaviour;
using StateMachineBase;
using UnityEngine;

namespace Logic.AnimalsStateMachine.States
{
    public class Wait : State
    {
        private readonly AnimalAnimator _animator;

        public Wait(AnimalAnimator animator) : base(animator)
        {
            _animator = animator;
            Debug.Log("Wait");
        }

        protected override void OnEnter()
        {
            _animator.SetIdle();
        }
    }
}