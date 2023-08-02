using NTC.Global.Cache;
using NTC.Global.Cache.Interfaces;
using UnityEngine;

namespace DelayRoutines
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

        protected override void OnPlay()
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

        public void Pause()
        {
            if (IsActive)
            {
                IsActive = false;
                _globalUpdate.RemoveRunSystem(this);
            }
        }

        public void Resume()
        {
            if (IsActive == false)
            {
                IsActive = true;
                _globalUpdate.AddRunSystem(this);
            }
        }
    }
}