using Logic.Wallet;
using NaughtyAttributes;
using Observables;
using UnityEngine;

namespace Player
{
    public class HeroWallet : MonoBehaviour
    {
        private readonly CompositeDisposable _disposables = new();
        
        private Wallet _wallet;
        public Wallet Wallet => _wallet;

        private void Awake()
        {
            _wallet = new Wallet();
             // LogMoneyChange();
             _wallet.TryAdd(100);
        }

        private void LogMoneyChange()
        {
            _disposables
                .Add(_wallet.Account
                    .Then((coins => print($"Current amount of coins: {coins}"))));
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }

        [Button("Add 100 Coins", EButtonEnableMode.Playmode)]
        private void AddCoins()
        {
            _wallet.TryAdd(100);
        }
    }
}