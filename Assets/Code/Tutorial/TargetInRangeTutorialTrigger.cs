using System;
using NTC.Global.Cache;
using UnityEngine;

namespace Tutorial
{
    [RequireComponent(typeof(BoxCollider))]
    public class TargetInRangeTutorialTrigger : MonoCache, ITutorialTrigger
    {
        public event Action Triggered = () => { };

        private void OnTriggerEnter(Collider other)
        {
            Triggered.Invoke();
        }
    }
}