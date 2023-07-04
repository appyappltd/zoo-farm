using Data.ItemsData;
using Logic.Interactions;
using Logic.Storages.Items;
using UnityEngine;

namespace Logic.Storages
{
    [RequireComponent(typeof(TimerOperator))]
    public class ProductReceiver : MonoBehaviour
    {
        [SerializeField] private PlayerInteraction _playerInteraction;
        [SerializeField] private TimerOperator _timerOperator;
        [SerializeField] private ItemId _itemIdReceiving;
        [SerializeField, Min(.0f)] private float _receiveRate = .2f;
        [SerializeField] private ReceiverMode _mode;

        private IAddItem _receiver;
        private IGetItem _sender;

        private void Awake()
        {
            _timerOperator ??= GetComponent<TimerOperator>();

            if (_mode == ReceiverMode.Collector)
            {
                _receiver = GetComponent<IAddItem>();
            }
            else
            {
                _sender = GetComponent<IGetItem>();
            }
            
            _timerOperator.SetUp(_receiveRate, OnReceive);
        }

        private void OnEnable()
        {
            _playerInteraction.Interacted += OnInteracted;
            _playerInteraction.Canceled += OnCanceled;
        }

        private void OnDisable()
        {
            _playerInteraction.Interacted -= OnInteracted;
            _playerInteraction.Canceled -= OnCanceled;
        }

        private void OnReceive()
        {
            if (_sender.CanGet(_itemIdReceiving, out IItem item))
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

        private void OnInteracted(HeroProvider heroProvider)
        {
            if (_mode == ReceiverMode.Collector)
            {
                _sender = heroProvider.Inventory;
            }
            else
            {
                _receiver = heroProvider.Inventory;
            }

            _timerOperator.Restart();
        }
    }
}
