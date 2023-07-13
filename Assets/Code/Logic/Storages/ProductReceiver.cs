using Data.ItemsData;
using Logic.Interactions;
using Logic.Player;
using Logic.Storages.Items;
using NaughtyAttributes;
using Tools;
using UnityEngine;

namespace Logic.Storages
{
    [RequireComponent(typeof(TimerOperator))]
    public class ProductReceiver : MonoBehaviour
    {
        [SerializeField] private PlayerInteraction _playerInteraction;
        [SerializeField] private TimerOperator _timerOperator;
        [SerializeField] private ItemId _itemIdFilter;
        [SerializeField, Min(.0f)] private float _receiveRate = .2f;
        [SerializeField] private ReceiverMode _mode;

        [SerializeField] private bool _isRemoteInit;

        [ShowIf("IsCollector")] [DisableIf("_isRemoteInit")]
        [SerializeField] [RequireInterface(typeof(IAddItem))] private MonoBehaviour _adder;
        
        [HideIf("IsCollector")] [DisableIf("_isRemoteInit")]
        [SerializeField] [RequireInterface(typeof(IGetItem))] private MonoBehaviour _remover;
        
        private IAddItem _receiver;
        private IGetItem _sender;

        private bool IsCollector => _mode == ReceiverMode.Collector;
        
        private void Awake()
        {
            _timerOperator.SetUp(_receiveRate, OnBeginReceive);
            _timerOperator ??= GetComponent<TimerOperator>();

            if (_isRemoteInit)
                return;

            if (_mode == ReceiverMode.Collector)
            {
                _receiver = (IAddItem) _adder;
            }
            else
            {
                _sender = (IGetItem) _remover;
            }
        }

        private void OnEnable()
        {
            if(_isRemoteInit == false)
                Subscribe();
        }

        private void OnDisable()
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

        public void Construct(IInventory inventory)
        {
            if (_mode == ReceiverMode.Collector)
            {
                _receiver = inventory;
            }
            else
            {
                _sender = inventory;
            }
            
            Subscribe();
        }
        
        private void OnBeginReceive()
        {
            if (_sender.TryPeek(_itemIdFilter, out IItem item))
            {
                if (_receiver.CanAdd(item))
                {
                    Receive();
                    _timerOperator.Restart();
                }
            }
        }

        private void Receive()
        {
            IItem receivedItem = _sender.Get();
            _receiver.Add(receivedItem);
        }

        private void OnCanceled() =>
            _timerOperator.Pause();

        private void OnInteracted(Hero hero)
        {
            if (_mode == ReceiverMode.Collector)
            {
                _sender = hero.Inventory;
            }
            else
            {
                _receiver = hero.Inventory;
            }

            OnBeginReceive();
        }
    }
}
