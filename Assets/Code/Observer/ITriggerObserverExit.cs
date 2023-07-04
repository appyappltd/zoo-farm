using System;
using UnityEngine;

namespace Observer
{
    public interface ITriggerObserverExit : ITriggerObserver
    {
        event Action<Collider> Exited;
    }
}