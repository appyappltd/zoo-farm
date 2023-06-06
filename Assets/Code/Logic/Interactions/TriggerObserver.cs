using System;
using UnityEngine;

namespace Logic.Interactions
{
    [RequireComponent(typeof(BoxCollider))]
    public class TriggerObserver : MonoBehaviour
    {
        public event Action<GameObject> Enter = c => { };
        public event Action<GameObject> Exit = c => { };

        private void OnTriggerEnter(Collider other) =>
            Enter.Invoke(other.gameObject);
        private void OnTriggerExit(Collider other) =>
            Exit.Invoke(other.gameObject);
    }
}
