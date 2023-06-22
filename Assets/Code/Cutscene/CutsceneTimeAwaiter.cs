using NTC.Global.Cache;
using Tools;
using UnityEngine;

namespace Cutscene
{
    public class CutsceneTimeAwaiter : CutsceneModule
    {
        private readonly GlobalUpdate _globalUpdate;
        private readonly Timer _timer;

        public CutsceneTimeAwaiter(float waitTime, GlobalUpdate globalUpdate)
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