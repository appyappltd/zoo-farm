using System;
using NTC.Global.Cache;
using UnityEngine;

namespace Cutscene
{
    [RequireComponent(typeof(BoxCollider))]
    public class TargetInRangeCutsceneTrigger : MonoCache, ICutsceneTrigger
    {
        public event Action Triggered = () => { };

        private void OnTriggerEnter(Collider other)
        {
            Triggered.Invoke();
        }
    }
}