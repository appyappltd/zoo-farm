using Logic.Storages;
using Logic.Storages.Items;
using Logic.VolunteersStateMachine;
using UnityEngine;

namespace Logic.Volunteers
{
    public class Volunteer : MonoBehaviour
    {
        [SerializeField] private InventoryHolder _inventoryHolder;
        [SerializeField] private VolunteerStateMachine _stateMachine;
        [SerializeField] private InventoryAnimatorObserver _handsAnimator;
        [SerializeField] private Storage _storage;

        private bool _isFree;
        private bool _isReadyTransmitting;
        private Transform _queuePlace;

        public IInventory Inventory => _inventoryHolder.Inventory;
        public VolunteerStateMachine StateMachine => _stateMachine;
        public bool IsFree => _isFree;
        public bool IsReadyTransmitting => _isReadyTransmitting;
        public Vector3 QueuePosition => _queuePlace.position;

        public void Awake()
        {
            _inventoryHolder.Construct();
            _handsAnimator.Construct(_inventoryHolder.Inventory);
            _storage.Construct(_inventoryHolder.Inventory);

            Inventory.Added += OnAdd;
            Inventory.Removed += OnRemove;
        }

        public void UpdateQueuePlace(Transform place)
        {
            _queuePlace = place;
        }
        
        private void OnRemove(IItem _)
        {
            _isReadyTransmitting = false;
        }

        private void OnDestroy() =>
            Inventory.Added -= OnAdd;

        private void OnAdd(IItem _) =>
            _isFree = false;

        public void Reload() =>
            _isFree = true;

        public void ReadyTransmitting() =>
            _isReadyTransmitting = true;
    }
}