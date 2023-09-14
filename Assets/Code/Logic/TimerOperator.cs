using System;
using NTC.Global.Cache;
using Tools.Timers;
using UnityEngine;

namespace Logic
{
    public class TimerOperator : MonoCache
    {
        private ITimer _timer;
        private Action _callBack;
        private bool _isEnabled;

        private void Awake() =>
            enabled = false;

        protected override void Run() =>
            _timer.Tick(Time.deltaTime);

        public void SetUp(float duration, Action action)
        {
            _timer = new Timer(duration, OnTimeIsOn);
            _callBack = action;
            Reset();
        }
        
        public void SetUp(GradualTimerSetup setup, Action action)
        {
            _timer = new GradualTimer(setup.InitialDelay, setup.FinalDelay, setup.DelaySteps, setup.DelayCurve, OnTimeIsOn);
            _callBack = action;
            Reset();
        }

        public void Pause()
        {
            if (enabled)
                enabled = false;
        }

        public void Play()
        {
            if (enabled == false)
                enabled = true;
        }

        public void Restart()
        {
            enabled = true;
            _timer.Restart();
        }

        public void Reset() =>
            _timer.Reset();

        private void OnTimeIsOn()
        {
            enabled = false;
            _callBack.Invoke();
        }
    }
}