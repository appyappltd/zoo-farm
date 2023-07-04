using System;
using Logic;
using Logic.Interactions;
using Logic.Storages;
using Logic.Storages.Items;
using Observer;
using UnityEngine;

namespace Data.ItemsData
{
    [RequireComponent(typeof(TriggerObserver))]
    public class DropItem : MonoBehaviour
    {
        public event Action<HandItem> PickedUp;

        private PlayerInteraction _playerInteraction;

        private IItem _itemData;

        private void OnEnable()
        {
            _playerInteraction.Interacted += OnInteracted;
        }

        private void OnDisable()
        {
            _playerInteraction.Interacted -= OnInteracted;
        }

        private void OnInteracted(HeroProvider heroProvider)
        {
            Inventory inventory = heroProvider.Inventory;

            if (inventory.TryAdd())
            {
                var item = Instantiate(_itemData.Hand, transform.position, transform.rotation);
                inventory.Add(item);
                PickedUp?.Invoke(item);
                Destroy(gameObject, .01f);
            }
        }
    }
}