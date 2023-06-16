using System;
using NTC.Global.Cache;
using Tools;
using UnityEngine;

namespace Logic
{
    public class TimerOperator : MonoCache
    {
        private Timer _timer;
        private Action _callBack;
        private bool _isEnabled;

        private void Awake() =>
            enabled = false;

        protected override void Run() =>
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            _timer.Tick(Time.deltaTime);

        public void SetUp(float duration, Action action)
        {
            _timer = new Timer(duration, OnTimeIsOn);
            _callBack = action;
        }

        public void Pause() =>
            enabled = false;

        public void Play() =>
            enabled = true;

        public void Restart()
        {
            enabled = false;
            _timer.Reset();
            enabled = true;
        }

        private void Reset() =>
            _timer.Reset();

        private void OnTimeIsOn()
        {
            enabled = false;
            _callBack?.Invoke();
        }
    }
}