using NTC.Global.Cache;
using NTC.Global.Cache.Interfaces;
using UnityEngine;

namespace DelayRoutines
{
    public abstract class Awaiter : Routine, IRunSystem
    {
        private readonly GlobalUpdate _globalUpdate;

        protected Awaiter(GlobalUpdate globalUpdate) =>
            _globalUpdate = globalUpdate;

        public abstract void OnRun();

        protected override void OnPlay() =>
            Activate();

        protected void Activate()
        {
            Debug.Log("activate");
            _globalUpdate.AddRunSystem(this);
        }

        protected void Deactivate()
        {
            Debug.Log("deactivate");
            _globalUpdate.RemoveRunSystem(this);
        }

        public void Pause()
        {
            if (IsActive)
            {
                IsActive = false;
                Deactivate();
            }
        }

        public void Resume()
        {
            if (IsActive == false)
            {
                IsActive = true;
                Activate();
            }
        }
    }
}