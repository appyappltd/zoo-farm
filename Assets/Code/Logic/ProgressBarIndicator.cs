using System;
using System.Diagnostics;
using NaughtyAttributes;
using NTC.Global.Cache;
using Progress;
using UnityEngine;

namespace Logic
{
    public class ProgressBarIndicator : MonoCache
    {
        [SerializeField] [Range(0, 100f)] private float _maxValue;
        [SerializeField] [Range(0, 10f)] private float _reduceSpeed;
        
        private ProgressBar _progressBar;

        public IProgressBar ProgressBar => _progressBar;

        private void Awake() =>
            _progressBar = new ProgressBar(_maxValue, _maxValue);

        protected override void OnEnabled()
        {
            _progressBar.Empty += Disable;
        }

        protected override void OnDisabled()
        {
            _progressBar.Empty -= Disable;
        }

        private void OnValidate() =>
            enabled = _reduceSpeed > 0;

        private void Disable() =>
            enabled = false;

        protected override void Run() =>
            _progressBar.Spend(Time.deltaTime * _reduceSpeed);

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