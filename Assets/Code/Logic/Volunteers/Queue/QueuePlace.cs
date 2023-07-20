using System;
using Logic.Interactions;
using Logic.Player;
using Logic.Storages;
using UnityEngine;

namespace Logic.Volunteers.Queue
{
    public class QueuePlace : MonoBehaviour
    {
        [SerializeField] private PlayerInteraction _playerInteraction;
        [SerializeField] private GameObject _interactionView;
        [SerializeField] private ProductReceiver _productReceiver;

        public event Action<QueuePlace> Vacated = q => { };

        private void Awake() =>
            _playerInteraction.Interacted += OnInteracted;

        private void OnDestroy() =>
            _playerInteraction.Interacted -= OnInteracted;

        public void Construct(IInventory inventory) =>
            _productReceiver.Construct(inventory);

        public void Show()
        {
            _playerInteraction.gameObject.SetActive(true);
            _interactionView.SetActive(true);
        }

        public void Hide()
        {
            _playerInteraction.gameObject.SetActive(false);
            _interactionView.SetActive(false);
        }

        private void OnInteracted(Hero _)
        {
            Vacated.Invoke(this);
        }
    }
}