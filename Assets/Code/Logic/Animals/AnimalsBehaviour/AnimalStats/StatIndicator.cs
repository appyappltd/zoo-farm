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

        private ProgressBar _progressBar;
        private ProgressBarOperator _progressBarOperator;

        public IProgressBar ProgressBar => _progressBar;

        private void OnDestroy()
        {
            _progressBar.Empty -= Disable;
        }

        public void Construct(float startValue)
        {
            enabled = false;
            
            _progressBar = new ProgressBar(_maxValue, startValue);
            _progressBarOperator = new ProgressBarOperator(_progressBar, _changeSpeed * _speedModifier, Convert.ToBoolean(_turn));
            
            _progressBar.Empty += Disable;
        }

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