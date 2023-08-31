using System;
using Infrastructure.Factory;
using Logic.Interactions;
using Logic.Player;
using Logic.Spawners;
using Logic.Translators;
using Observables;
using Services;
using Services.Pools;
using Tools.Timers;
using UnityEngine;

namespace Logic.Payment
{
    [RequireComponent(typeof(TimerOperator))]
    [RequireComponent(typeof(RunTranslator))]
    public class SmoothConsumer : MonoBehaviour, IConsumer
    {
        private readonly Observable<int> _leftCoinsToPay = new Observable<int>();
        
        [SerializeField] private HeroInteraction _playerInteraction;
        [SerializeField] private GradualTimerPreset _timerPreset;
        [SerializeField, Min(0)] private int _currencyPerTick = 1;

        private IPoolService _poolService;
        private int _defaultCost;
        private VisualTranslatorsSpawner _spawner;
        private RunTranslator _translator;
        private IWallet _wallet;
        private TimerOperator _timerOperator;

        public event Action Bought = () => { };
        
        public Observables.IObservable<int> LeftCoinsToPay => _leftCoinsToPay;

        private void Awake()
        {
            _poolService = AllServices.Container.Single<IPoolService>();
            _translator = GetComponent<RunTranslator>();
            _timerOperator = GetComponent<TimerOperator>();
            
            Subscribe();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        public void SetCost(int buildCost)
        {
            _defaultCost = buildCost;
            _timerOperator.SetUp(_timerPreset.GetSetup(), OnPay);
            _leftCoinsToPay.Value = buildCost;
        }

        private void Init(Hero hero)
        {
            _spawner = new VisualTranslatorsSpawner(() =>
                    AllServices.Container.Single<IGameFactory>().CreateVisual(VisualType.Money,
                        Quaternion.identity),
                10, _translator, hero.transform, transform, _poolService);
            
            _playerInteraction.Interacted -= Init;
        }

        private void Subscribe()
        {
            _playerInteraction.Interacted += Init;
            _playerInteraction.Interacted += BeginTransaction;
            _playerInteraction.Canceled += CancelTransaction;
        }

        private void Unsubscribe()
        {
            _playerInteraction.Interacted -= BeginTransaction;
            _playerInteraction.Canceled -= CancelTransaction;
        }

        private void CancelTransaction() =>
            _timerOperator.Pause();

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

            _wallet = hero.Wallet;
            _timerOperator.Reset();
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
