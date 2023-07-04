using Logic;
using Logic.Interactions;
using Tutorial.StaticTriggers;
using UnityEngine;

namespace Tutorial
{
    public class TutorialInteractedTriggerContainer : MonoBehaviour
    {
        [SerializeField] private PlayerInteraction _playerInteraction;
        [SerializeField] private TutorialTriggerStatic _staticTrigger;

        private void OnEnable() =>
            _playerInteraction.Interacted += OnComplete;

        private void OnDisable() =>
            _playerInteraction.Interacted -= OnComplete;

        private void OnComplete(HeroProvider sender) =>
            _staticTrigger.Trigger(sender.gameObject);
    }
}