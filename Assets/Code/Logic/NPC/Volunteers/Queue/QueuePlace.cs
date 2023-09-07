using System;
using Logic.Interactions;
using Logic.Interactions.Views;
using Logic.Player;
using Logic.Storages;
using NTC.Global.System;
using UnityEngine;

namespace Logic.NPC.Volunteers.Queue
{
    public class QueuePlace : MonoBehaviour
    {
        [SerializeField] private HumanInteraction _playerInteraction;
        [SerializeField] private GameObject _interactionView;
        [SerializeField] private InteractionView _interactionSine;
        [SerializeField] private InteractionViewRejector _interactionRejector;
        [SerializeField] private ProductReceiver _productReceiver;

        public event Action<QueuePlace> Vacated = _ => { };

        private void Awake() =>
            _playerInteraction.Interacted += OnInteracted;

        private void OnDestroy() =>
            _playerInteraction.Interacted -= OnInteracted;

        public void Construct(IInventory inventory) =>
            _productReceiver.Construct(inventory);

        public void Show()
        {
            _playerInteraction.Enable();
            _playerInteraction.gameObject.Enable();
            _interactionView.Enable();
            _interactionRejector.SetDefault();
            _interactionSine.SetDefault();
        }

        public void Hide()
        {
            _playerInteraction.Disable();
            _playerInteraction.gameObject.Disable();
            _interactionView.Disable();
        }

        private void OnInteracted(Human _)
        {
            Hide();
            Vacated.Invoke(this);
        }
    }
}