using System;
using Logic.Interactions;
using UnityEngine;

namespace Tutorial
{
    public class InteractionTutorialTrigger : MonoBehaviour, ITutorialTrigger
    {
        [SerializeField] private Delay _delay;

        public event Action Triggered = () => { };

        private void Awake() =>
            _delay.Complete += OnDelayComplete;

        private void OnDestroy() =>
            _delay.Complete -= OnDelayComplete;

        private void OnDelayComplete(GameObject _) =>
            Triggered.Invoke();
    }
}