using Logic.Payment;
using Services.Input;
using UnityEngine;

namespace Logic.Player
{
    public sealed class Hero : Human
    {
        [SerializeField] private WalletHolder _walletHolder;
        [SerializeField] private MaxIndicator _maxIndicator;
        [SerializeField] private PlayerMovement _playerMovement;

        private IWallet _wallet;

        public IWallet Wallet => _walletHolder.Wallet;

        public void Construct(IPlayerInputService inputService)
        {
            Init();
            _maxIndicator.Construct(InventoryHolder.Inventory);
            _playerMovement.Construct(inputService);
        }
    }
}