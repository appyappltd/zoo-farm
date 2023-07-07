using System;
using Infrastructure.Factory;
using Logic.Interactions;
using Logic.Player;
using Logic.Spawners;
using Logic.Translators;
using Observables;
using Services;
using UnityEngine;

namespace Logic.Payment
{
    [RequireComponent(typeof(TimerOperator))]
    [RequireComponent(typeof(RunTranslator))]
    public class Consumer : MonoBehaviour
    {
        private readonly Observable<int> _leftCoinsToPay = new Observable<int>();
        
        [SerializeField] private PlayerInteraction _playerInteraction;
        [SerializeField, Min(0f)] private float _paymentRate = 0.05f;
        [SerializeField, Min(0)] private int _currencyPerTick = 1;

        private int _defaultCost;
        private VisualTranslatorsSpawner _spawner;
        private RunTranslator _translator;
        private IWallet _wallet;
        private TimerOperator _timerOperator;

        public event Action Bought = () => { };
        public Observables.IObservable<int> LeftCoinsToPay => _leftCoinsToPay;

        private void Awake()
        {
            _translator = GetComponent<RunTranslator>();
            _timerOperator = GetComponent<TimerOperator>();
            _timerOperator.SetUp(_paymentRate, OnPay);
            _playerInteraction.Entered += Init;
        }

        private void OnEnable()
        {
            _playerInteraction.Interacted += BeginTransaction;
            _playerInteraction.Canceled += CancelTransaction;
        }

        private void OnDisable()
        {
            _playerInteraction.Interacted -= BeginTransaction;
            _playerInteraction.Canceled -= CancelTransaction;
        }

        private void CancelTransaction()
        {
            _timerOperator.Pause();
        }

        public void SetCost(int buildCost)
        {
            _defaultCost = buildCost;
            _leftCoinsToPay.Value = buildCost;
        }

        private void Init(Hero hero)
        {
            _spawner = new VisualTranslatorsSpawner(() =>
                    AllServices.Container.Single<IGameFactory>().CreateVisual(VisualType.Money,
                        Quaternion.identity,
                        new GameObject("Coins").transform),
                10, _translator, hero.transform, transform);
            _wallet = hero.Wallet;
            
            _playerInteraction.Entered -= Init;
        }

        private void OnPay()
        {
            if (_wallet.TrySpend(_currencyPerTick) == false)
                return;
            
            _leftCoinsToPay.Value -= _currencyPerTick;
            ITranslatable translatable = _spawner.Spawn().MainTranslatable;
            
            if (_leftCoinsToPay.Value <= 0)
            {
                translatable.End += Buy;
                return;
            }
                
            _timerOperator.Restart();
        }

        private void BeginTransaction(Hero hero)
        {
            if (_leftCoinsToPay.Value <= 0)
                return;

            _timerOperator.Restart();
        }

        private void Buy(ITranslatable translatable)
        {
            Bought.Invoke();
            translatable.End -= Buy;
            _leftCoinsToPay.Value = _defaultCost;
        }
    }
}
