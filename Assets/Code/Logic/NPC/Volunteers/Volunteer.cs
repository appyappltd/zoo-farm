using System;
using Data.ItemsData;
using Logic.NPC.Volunteers.Queue;
using Logic.NPC.Volunteers.VolunteersStateMachine;
using Logic.Player;
using Logic.Storages;
using Logic.Storages.Items;
using UnityEngine;

namespace Logic.NPC.Volunteers
{
    public class Volunteer : Human, IGetItem
    {
        [SerializeField] private VolunteerStateMachine _stateMachine;

        private bool _isFree;
        private bool _isReadyTransmitting;
        private QueuePlace _queuePlace;
        
        public VolunteerStateMachine StateMachine => _stateMachine;
        public bool IsFree => _isFree;
        public Vector3 QueuePosition => _queuePlace.transform.position;
        public Quaternion QueueRotation => _queuePlace.transform.rotation;

        public event Action<IItem> Removed = _ => { };
        
        private void Awake()
        {
            Init();
            Inventory.Added += OnAdd;
            Inventory.Removed += OnRemove;
        }

        private void OnDestroy() =>
            Inventory.Added -= OnAdd;

        public IItem Get()
        {
            IItem item = Inventory.Get();
            Removed.Invoke(item);
            return item;
        }

        public bool TryPeek(ItemFilter filter, out IItem item)
        {
            item = null;
            return _isReadyTransmitting && Inventory.TryPeek(new ItemFilter(ItemId.Animal), out item);
        }

        public bool TryGet(ItemFilter filter, out IItem result)
        {
            if (TryPeek(filter, out result))
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

        private void OnAdd(IItem _) =>
            _isFree = false;

    }
}