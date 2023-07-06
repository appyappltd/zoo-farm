using System;
using Data.ItemsData;
using Logic.Interactions;
using Logic.Storages.Items;
using UnityEngine;

namespace Logic.Storages
{
    public class Trash : MonoBehaviour, IAddItemObserver, IGetItemObserver, IInventoryProvider
    {
        [SerializeField] private Storage _storage;
        [SerializeField] private PlayerInteraction _playerInteraction;

        public event Action<IItem> Added = i => { };
        public event Action<IItem> Removed = i => { };

        public IGetItemObserver GetItemObserver => this;
        public IAddItemObserver AddItemObserver => this;

        private void OnEnable()
        {
            _playerInteraction.Interacted += OnInteracted;
            _storage.Replenished += OnReplenished;
        }

        private void OnInteracted(HeroProvider provider)
        {
            if (provider.Inventory.TryGet(ItemId.All, out IItem item))
            {
                Added.Invoke(item);
            }
        }

        private void OnReplenished(IItem item)
        {
            item.Mover.Ended += OnMoveEnded;

            void OnMoveEnded()
            {
                item.Mover.Ended -= OnMoveEnded;
                Removed.Invoke(item);
                item.Destroy();
            }
        }
    }
}