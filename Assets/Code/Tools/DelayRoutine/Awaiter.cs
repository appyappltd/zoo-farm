using NTC.Global.Cache;
using NTC.Global.Cache.Interfaces;
using UnityEngine;

namespace Tools.DelayRoutine
{
    public sealed class Awaiter : Routine, IRunSystem
    {
        private readonly GlobalUpdate _globalUpdate;
        private readonly float _waitTime;

        private float _elapsedTime;

        public Awaiter(float waitTime, GlobalUpdate globalUpdate)
        {
            _waitTime = waitTime;
            _globalUpdate = globalUpdate;
        }

        public override void Play()
        {
            _elapsedTime = _waitTime;
            _globalUpdate.AddRunSystem(this);
        }

        public void OnRun()
        {
            _elapsedTime -= Time.deltaTime;

            if (_elapsedTime <= 0)
            {
                _globalUpdate.RemoveRunSystem(this);
                Next();
            }
        }
    }
}