﻿using System;

namespace Tools.Timers
{
    public class Timer : ITimer
    {
        private readonly float _duration;
        private readonly Action _onTimeIsOn;

        private float _elapsedTime;

        public bool IsActive => _elapsedTime >= 0;

        public Timer(float duration, Action onTimeIsOn)
        {
            _duration = duration;
            _elapsedTime = duration;
            _onTimeIsOn = onTimeIsOn;
        }

        public void Tick(float deltaTime)
        {
            if (IsActive == false)
                return;

            _elapsedTime -= deltaTime;

            if (_elapsedTime <= 0)
                _onTimeIsOn?.Invoke();
        }

        public void Restart() =>
            Reset();

        public void Reset() =>
            _elapsedTime = _duration;
    }
}