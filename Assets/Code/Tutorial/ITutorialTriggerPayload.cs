using System;
using UnityEngine;

namespace Tutorial
{
    public interface ITutorialTriggerPayload : ITutorialTrigger
    {
        public event Action<GameObject> TriggeredPayload;
    }
}