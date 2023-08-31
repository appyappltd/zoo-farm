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

        public event Action Bought = () => { };

        public Observables.IObservable<int> LeftCoinsToPay => _leftCoinsToPay;

        private void Awake() =>
            _playerInteraction.Interacted += Buy;

        private void OnDestroy() =>
            _playerInteraction.Interacted -= Buy;

        public void SetCost(int buildCost)
        {
            if (buildCost < 0)
                throw new ArgumentOutOfRangeException(nameof(buildCost));
            
            _leftCoinsToPay.Value = buildCost;
        }
        
        private void Buy(Hero hero)
        {
            if (hero.Wallet.TrySpend(_leftCoinsToPay.Value))
            {
                _leftCoinsToPay.Value -= _leftCoinsToPay.Value;
                Bought.Invoke();
                return;
            }

#if DEBUG
            Debug.LogWarning("Not enough money");
#endif
        }
    }
}