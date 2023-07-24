using System;
using Data.ItemsData;
using Logic.Storages;
using Logic.Storages.Items;
using Logic.Volunteers.Queue;
using Logic.VolunteersStateMachine;
using UnityEngine;

namespace Logic.Volunteers
{
    public class Volunteer : MonoBehaviour, IGetItem
    {
        [SerializeField] private InventoryHolder _inventoryHolder;
        [SerializeField] private VolunteerStateMachine _stateMachine;
        [SerializeField] private InventoryAnimatorObserver _handsAnimator;
        [SerializeField] private Storage _storage;

        private bool _isFree;
        private bool _isReadyTransmitting;
        private QueuePlace _queuePlace;

        public IInventory Inventory => _inventoryHolder.Inventory;
        public VolunteerStateMachine StateMachine => _stateMachine;
        public bool IsFree => _isFree;
        public Vector3 QueuePosition => _queuePlace.transform.position;
        public Quaternion QueueRotation => _queuePlace.transform.rotation;

        public event Action<IItem> Removed = i => { };
        
        private void Awake()
        {
            _inventoryHolder.Construct();
            _handsAnimator.Construct(_inventoryHolder.Inventory);
            _storage.Construct(_inventoryHolder.Inventory);

            Inventory.Added += OnAdd;
            Inventory.Removed += OnRemove;
        }

        public IItem Get()
        {
            IItem item = Inventory.Get();
            Removed.Invoke(item);
            return item;
        }

        public bool TryPeek(ItemId byId, out IItem item)
        {
            item = null;
            return _isReadyTransmitting && Inventory.TryPeek(ItemId.Animal, out item);
        }

        public bool TryGet(ItemId byId, out IItem result)
        {
            if (TryPeek(byId, out result))
            {
                result = Get();
                return true;
            }

            return false;
        }

        public void UpdateQueuePlace(QueuePlace place) =>
            _queuePlace = place;

        public void Reload() =>
            _isFree = true;

        public void ActivateTransmitting()
        {
            _isReadyTransmitting = true;
            _queuePlace.Show();
        }
        
        public void DeactivateTransmitting()
        {
            _isReadyTransmitting = false;
            _queuePlace.Hide();
        }

        private void OnRemove(IItem _) =>
            _isReadyTransmitting = false;

        private void OnDestroy() =>
            Inventory.Added -= OnAdd;

        private void OnAdd(IItem _) =>
            _isFree = false;

    }
}