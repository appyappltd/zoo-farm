using Logic.AnimalsBehaviour.AnimalStats;
using MonoStateMachine;
using UnityEngine;

namespace Logic.AnimalsBehaviour.States
{
    public class AnimalEatState : State
    {
        [SerializeField] private AnimalAnimator _animator;
        [SerializeField] private ProgressBarIndicator _satiety;
        [SerializeField] private float _satietySpendRate;

        protected override void OnEnter()
        {
            _animator.SetEat();
            _satiety.enabled = false;
        }

        protected override void OnExit()
        {
            _satiety.enabled = true;
        }

        protected override void Run()
        {
            _satiety.ProgressBar.Replenish(Time.deltaTime * _satietySpendRate);
        }
    }
}