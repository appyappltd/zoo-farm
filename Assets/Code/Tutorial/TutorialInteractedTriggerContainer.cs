using Logic.Interactions;
using Logic.Player;
using Tutorial.StaticTriggers;
using UnityEngine;

namespace Tutorial
{
    public class TutorialInteractedTriggerContainer : MonoBehaviour
    {
        [SerializeField] private HumanInteraction _playerInteraction;
        [SerializeField] private TutorialTriggerScriptableObject _staticTrigger;

        private void OnEnable() =>
            _playerInteraction.Interacted += OnComplete;

        private void OnDisable() =>
            _playerInteraction.Interacted -= OnComplete;

        private void OnComplete(Human sender) =>
            _staticTrigger.Trigger(sender.gameObject);
    }
}