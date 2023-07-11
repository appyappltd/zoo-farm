using Logic.Interactions;
using Logic.Player;
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

        private void OnComplete(Hero sender) =>
            _staticTrigger.Trigger(sender.gameObject);
    }
}