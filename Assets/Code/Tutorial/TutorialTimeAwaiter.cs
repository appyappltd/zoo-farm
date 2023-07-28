using NTC.Global.Cache;
using Tools.Timers;
using UnityEngine;

namespace Tutorial
{
    public class TutorialTimeAwaiter : TutorialModule
    {
        private readonly GlobalUpdate _globalUpdate;
        private readonly Timer _timer;

        public TutorialTimeAwaiter(float waitTime, GlobalUpdate globalUpdate)
        {
            _globalUpdate = globalUpdate;

            _timer = new Timer(waitTime, OnComplete);
        }

        private void OnComplete()
        {
            _globalUpdate.RemoveRunSystem(this);
            Complete();
        }

        public override void Play() =>
            _globalUpdate.AddRunSystem(this);

        public override void OnRun() =>
            _timer.Tick(Time.deltaTime);
    }
}