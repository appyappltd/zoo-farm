using MonoStateMachine;
using UnityEngine;

namespace Logic.AnimalsBehaviour.States
{
    public class AnimalRestState : State
    {
        [Header("Components")]
        [SerializeField] private AnimalAnimator _animator;
        [SerializeField] private IndicatorProgressBar _vitality;
        [SerializeField] private IndicatorProgressBar _peppiness;
        [SerializeField] private IndicatorProgressBar _satiety;

        [Header("Settings")] [Space]
        [SerializeField] private float _restTime;

        [SerializeField] private float _saturationRecoveryRate;
        


        protected override void OnEnabled() =>
            _animator.SetRest();

        protected override void Run()
        {
            base.Run();
        }
    }
}