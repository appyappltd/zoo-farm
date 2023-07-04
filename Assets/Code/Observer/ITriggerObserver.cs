using System;
using UnityEngine;

namespace Observer
{
    public interface ITriggerObserver
    {
        event Action<Collider> Entered;
    }
}