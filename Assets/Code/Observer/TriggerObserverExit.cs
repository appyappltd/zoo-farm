using System;
using UnityEngine;

namespace Observer
{
    public class TriggerObserverExit : TriggerObserver, ITriggerObserverExit
    {
        public event Action<Collider> Exited = c => { };

        private void OnTriggerExit(Collider other) =>
            Exited.Invoke(other);
    }
}