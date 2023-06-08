using NaughtyAttributes;
using NTC.Global.Cache;
using Progress;
using UnityEngine;

namespace Logic
{
    public class IndicatorProgressBar : MonoCache, IProgressBarView
    {
        [SerializeField] [Range(0, 100f)] private float _maxValue;
        [SerializeField] [Range(0, 10f)] private float _reduceSpeed;
        
        private ProgressBar _progressBar;

        public float Max => _progressBar.Max;
        public float Current => _progressBar.Current;
        public float CurrentNormalized => _progressBar.Current / _progressBar.Max;
        public bool IsEmpty => _progressBar.IsEmpty;
        public float ReduceSpeed => _progressBar.ReduceSpeed;

        private void Awake() =>
            _progressBar = new ProgressBar(_maxValue, _maxValue, _reduceSpeed);

        private void OnValidate()
        {
            if (_progressBar == null)
                return;

            if (Mathf.Approximately(_reduceSpeed, _progressBar.ReduceSpeed) == false)
                _progressBar.SetReduceSpeed(_reduceSpeed);

            if (Mathf.Approximately(_maxValue, _progressBar.Max) == false)
                _progressBar = new ProgressBar(_maxValue, _progressBar.Current, _reduceSpeed);
        }

        protected override void Run()
        {
            _progressBar.Tick(Time.deltaTime);

            if (_progressBar.IsEmpty)
                enabled = false;
        }

        public void SetReduceSpeed(float newSpeed) =>
            _progressBar.SetReduceSpeed(newSpeed);

        public void Replenish(float value)
        {
            _progressBar.Replenish(value);
            enabled = !_progressBar.IsEmpty;

        }
        
        [Button("ReplenishFull", EButtonEnableMode.Playmode)]
        private void Replenish() =>
            Replenish(_maxValue);
    }
}