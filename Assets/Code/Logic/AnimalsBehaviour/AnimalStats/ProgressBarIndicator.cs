using System.Diagnostics;
using NaughtyAttributes;
using NTC.Global.Cache;
using Progress;
using UnityEngine;

namespace Logic.AnimalsBehaviour.AnimalStats
{
    public class ProgressBarIndicator : MonoCache
    {
        [SerializeField] [Range(0, 100f)] private float _maxValue;
        [SerializeField] [Range(0, 10f)] private float _reduceSpeed;

        private ProgressBar _progressBar;
        private ProgressBarOperator _progressBarOperator;

        public IProgressBar ProgressBar => _progressBar;

        private void Awake()
        {
            _progressBar = new ProgressBar(_maxValue, _maxValue);
            _progressBarOperator = new ProgressBarOperator(_progressBar, _reduceSpeed, true);
        }

        protected override void OnEnabled() =>
            _progressBar.Empty += Disable;

        protected override void OnDisabled() =>
            _progressBar.Empty -= Disable;

        private void OnValidate() =>
            enabled = _reduceSpeed > 0;

        private void Disable() =>
            enabled = false;

        protected override void Run() =>
            _progressBarOperator.Update(Time.deltaTime);

        [Conditional("UNITY_EDITOR")]
        private void Replenish(float value)
        {
            _progressBar.Replenish(value);
            enabled = !_progressBar.IsEmpty;
        }

        [Conditional("UNITY_EDITOR")]
        [Button("ReplenishFull", EButtonEnableMode.Playmode)]
        private void Replenish() =>
            Replenish(_maxValue);
    }
}