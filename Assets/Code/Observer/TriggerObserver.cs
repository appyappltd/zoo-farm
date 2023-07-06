using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Observer
{
    public class TriggerObserver : MonoBehaviour, ITriggerObserver
    {
        public event Action<Collider> Entered = c => { };

        private void Awake()
        {
            Collider collider = GetComponent<Collider>();

            if (collider.IsUnityNull())
                throw new NullReferenceException("Collider for Trigger Observer is not set");

            collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other) =>
            Entered?.Invoke(other);
    }
}