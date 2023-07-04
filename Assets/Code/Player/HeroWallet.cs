using Logic.Payment;
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