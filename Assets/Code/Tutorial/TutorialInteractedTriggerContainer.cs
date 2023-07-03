using Logic.Interactions;
using Tutorial.StaticTriggers;
using UnityEngine;

namespace Tutorial
{
    public class TutorialInteractedTriggerContainer : MonoBehaviour
    {
        [SerializeField] private Delay _delay;
        [SerializeField] private TutorialTriggerStatic _staticTrigger;

        private void OnEnable() =>
            _delay.Complete += OnComplete;

        private void OnDisable() =>
            _delay.Complete -= OnComplete;

        private void OnComplete(GameObject sender) =>
            _staticTrigger.Trigger(sender);
    }
}