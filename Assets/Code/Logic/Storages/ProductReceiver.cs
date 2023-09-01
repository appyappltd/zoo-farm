using System;
using AYellowpaper;
using Data.ItemsData;
using Logic.Foods.FoodSettings;
using Logic.Interactions;
using Logic.Player;
using Logic.Storages.Items;
using NaughtyAttributes;
using UnityEngine;

namespace Logic.Storages
{
    [RequireComponent(typeof(TimerOperator))]
    public class ProductReceiver : MonoBehaviour
    {
        [SerializeField] private HumanInteraction _playerInteraction;
        [SerializeField] private TimerOperator _timerOperator;
        [SerializeField] private ItemId _itemIdFilter;
        
        [ShowIf(nameof(_itemIdFilter), ItemId.Food)]
        [SerializeField] private FoodId _foodIdFilter;
        
        [SerializeField] private bool _isSerialReceive;

        [SerializeField] [ShowIf("_isSerialReceive")] [Min(.0f)]
        private float _receiveRate = .2f;

        [SerializeField] private ReceiverMode _mode;
        [SerializeField] private bool _isRemoteInit;

        [SerializeField]
        [ShowIf("_mode", ReceiverMode.Collector)]
        [DisableIf("_isRemoteInit")]
        [RequireInterface(typeof(IAddItem))]
        private MonoBehaviour _adder;

        [SerializeField]
        [ShowIf("_mode", ReceiverMode.Giver)]
        [DisableIf("_isRemoteInit")]
        [RequireInterface(typeof(IGetItem))]
        private MonoBehaviour _remover;

        private IAddItem _receiver;
        private IGetItem _sender;

        private ItemFilter _itemFilter;
        private Action _beginReceive;

        private void Awake()
        {
            _beginReceive = TryReceive;
            
            if (_isSerialReceive)
                _beginReceive += RestartTimer;
            
            _timerOperator ??= GetComponent<TimerOperator>();
            _timerOperator.SetUp(_receiveRate, _beginReceive);

            _itemFilter = new ItemFilter(_itemIdFilter, _foodIdFilter);
            
            if (_isRemoteInit)
                return;

            if (_mode == ReceiverMode.Collector)
                _receiver = (IAddItem) _adder;
            else
                _sender = (IGetItem) _remover;
        }

        private void Start()
        {
            if(_isRemoteInit == false)
                Subscribe();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            _playerInteraction.Interacted += OnInteracted;
            _playerInteraction.Canceled += OnCanceled;
        }

        private void Unsubscribe()
        {
            _playerInteraction.Interacted -= OnInteracted;
            _playerInteraction.Canceled -= OnCanceled;
        }

        public void Construct(IInventory selfInventory)
        {
            ApplySelfInventory(selfInventory);
            Subscribe();
        }

        public bool CanReceive(IInventory inventory)
        {
            ApplyRemoteInventory(inventory);
            return _sender.TryPeek(_itemFilter, out IItem item) && _receiver.CanAdd(item);
        }

        private void TryReceive()
        {
            if (_sender.TryPeek(_itemFilter, out IItem item) && _receiver.CanAdd(item))
                Receive();
        }

        private void ApplyRemoteInventory(IInventory remoteInventory)
        {
            if (_mode == ReceiverMode.Collector)
                _sender = remoteInventory;
            else
                _receiver = remoteInventory;
        }

        private void ApplySelfInventory(IInventory selfInventory)
        {
            if (_mode == ReceiverMode.Collector)
                _receiver = selfInventory;
            else
                _sender = selfInventory;
        }

        private void OnCanceled() =>
            _timerOperator.Pause();

        private void OnInteracted(Human hero)
        {
            ApplyRemoteInventory(hero.Inventory);
            _beginReceive.Invoke();
        }

        private void RestartTimer() =>
            _timerOperator.Restart();

        private void Receive()
        {
            IItem receivedItem = _sender.Get();
            _receiver.Add(receivedItem);
        }
    }
}