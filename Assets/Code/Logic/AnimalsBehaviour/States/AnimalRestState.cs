using Logic.AnimalsBehaviour.AnimalStats;
using MonoStateMachine;
using UnityEngine;

namespace Logic.AnimalsBehaviour.States
{
    public class AnimalRestState : State
    {
        [Header("Component References")]
        [SerializeField] private AnimalAnimator _animator;
        [SerializeField] private ProgressBarIndicator _vitality;
        [SerializeField] private ProgressBarIndicator _peppiness;
        [SerializeField] private ProgressBarIndicator _satiety;

        [Header("Rate Settings")] [Space]
        [SerializeField] private float _satietySpendRate;
        [SerializeField] private float _peppinessRecoveryRate;
        [SerializeField] private float _vitalityRecoveryRate;

        protected override void OnEnter()
        {
            _animator.SetRest();
            SetBarsActive(false);
        }

        protected override void OnExit()
        {
            SetBarsActive(true);
        }

        private void SetBarsActive(bool isActive)
        {
            _vitality.enabled = isActive;
            _peppiness.enabled = isActive;
            _satiety.enabled = isActive;
        }

        protected override void Run()
        {
            float deltaTime = Time.deltaTime;
            _vitality.ProgressBar.Replenish(deltaTime * _vitalityRecoveryRate);
            _peppiness.ProgressBar.Replenish(deltaTime * _peppinessRecoveryRate);
            _satiety.ProgressBar.Spend(deltaTime * _satietySpendRate);
        }
    }
}