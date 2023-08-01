using System;
using UnityEngine;

namespace Tools.Timers
{
    public class GradualTimer : ITimer
    {
        private readonly Action _timerCallback;
        private readonly float _initialDelay;
        private readonly float _finalDelay;
        private readonly float _delaySteps;
        private readonly AnimationCurve _delayCurve;

        private float _elapsedTime;
        private float _currentStep;
        private float _currentDelay;

        public bool IsActive => _elapsedTime > 0;

        public GradualTimer(float initialDelay, float finalDelay, float delaySteps, AnimationCurve delayCurve,
            Action timerCallback)
        {
            _initialDelay = initialDelay;
            _finalDelay = finalDelay;
            _elapsedTime = initialDelay;
            _delayCurve = delayCurve;
            _delaySteps = delaySteps;
            _timerCallback = timerCallback;

            Reset();
        }

        public void Restart()
        {
            _elapsedTime = 0;
        }

        public void Reset()
        {
            _elapsedTime = 0;
            _currentStep = 0;
            _currentDelay = LerpDelay();
        }

        public void Tick(float delta)
        {
            _elapsedTime += delta;

            if (_elapsedTime >= _currentDelay)
            {
                _timerCallback?.Invoke();
                _currentDelay = LerpDelay();
                _elapsedTime = 0;
            }
        }

        private float LerpDelay()
        {
            if (_currentStep >= _delaySteps)
                return _finalDelay;

            float steppedDelay = _delayCurve.Evaluate(_currentStep / _delaySteps);
            _currentStep++;
            return Mathf.Lerp(_initialDelay, _finalDelay, steppedDelay);
        }
    }
}