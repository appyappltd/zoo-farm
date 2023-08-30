using System;
using Logic.Interactions;
using Logic.Player;
using Observables;
using UnityEngine;

namespace Logic.Payment
{
    public class OnceConsumer : MonoBehaviour, IConsumer
    {
        private readonly Observable<int> _leftCoinsToPay = new Observable<int>();
        
        [SerializeField] private HeroInteraction _playerInteraction;

        private int _defaultCost;
        private IWallet _wallet;

        public event Action Bought = () => { };

        public Observables.IObservable<int> LeftCoinsToPay => _leftCoinsToPay;

        private void Awake()
        {
            _playerInteraction.Interacted += Buy;
        }

        private void OnDestroy()
        {
            _playerInteraction.Interacted -= Buy;
        }

        public void SetCost(int buildCost)
        {
            if (buildCost < 0)
                throw new ArgumentOutOfRangeException(nameof(buildCost));

            _defaultCost = buildCost;
        }
        
        private void Buy(Hero _)
        {
            if (_wallet.TrySpend(_defaultCost))
            {
                _leftCoinsToPay.Value -= _defaultCost;
                Bought.Invoke();
            }
        }
    }
}