using System;
using AYellowpaper;
using Data.ItemsData;
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

        private Action BeginReceive;

        private void Awake()
        {
            BeginReceive = TryReceive;
            
            if (_isSerialReceive)
                BeginReceive += RestartTimer;
            
            _timerOperator ??= GetComponent<TimerOperator>();
            _timerOperator.SetUp(_receiveRate, BeginReceive);

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
            return _sender.TryPeek(_itemIdFilter, out IItem item) && _receiver.CanAdd(item);
        }

        private void TryReceive()
        {
            if (_sender.TryPeek(_itemIdFilter, out IItem item))
                if (_receiver.CanAdd(item))
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
            BeginReceive.Invoke();
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