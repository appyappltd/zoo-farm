using System;
using System.Diagnostics;
using NaughtyAttributes;
using NTC.Global.Cache;
using Progress;
using UnityEngine;

namespace Logic.Animals.AnimalsBehaviour.AnimalStats
{
    public class StatIndicator : MonoCache
    {
        private readonly float _speedModifier = 0.1f;
        
        [SerializeField] [Range(0, 100f)] private float _maxValue;
        [SerializeField] [Range(0, 1f)] private float _changeSpeed;
        [SerializeField] private Turn _turn;
        [SerializeField] [Range(0, 100f)] private float _startValue;

        private ProgressBar _progressBar;
        private ProgressBarOperator _progressBarOperator;

        public IProgressBar ProgressBar => _progressBar;

        private void Awake()
        {
            _progressBar = new ProgressBar(_maxValue, _startValue);
            _progressBarOperator = new ProgressBarOperator(_progressBar, _changeSpeed * _speedModifier, Convert.ToBoolean(_turn));
        }

        protected override void OnEnabled() =>
            _progressBar.Empty += Disable;

        protected override void OnDisabled() =>
            _progressBar.Empty -= Disable;

        private void OnValidate() =>
            enabled = _changeSpeed > 0;

        public void Disable() =>
            enabled = false;
        
        public void Enable() =>
            enabled = true;

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