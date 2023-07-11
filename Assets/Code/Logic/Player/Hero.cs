using Logic.Bubble;
using Logic.Payment;
using Logic.Storages;
using Player;
using Services.Input;
using UnityEngine;

namespace Logic.Player
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private InventoryHolder _inventoryHolder;
        [SerializeField] private WalletHolder _walletHolder;
        [SerializeField] private MaxIndicator _maxIndicator;
        [SerializeField] private InventoryAnimatorObserver _handsAnimator;
        [SerializeField] private Storage _storage;
        
        [SerializeField] private PlayerMovement _playerMovement;
        
        private IWallet _wallet;
        private IInventory _inventory;
        
        public IWallet Wallet => _walletHolder.Wallet;
        public IInventory Inventory => _inventoryHolder.Inventory;

        public void Construct(IPlayerInputService inputService)
        {
            _playerMovement.Construct(inputService);
            _inventoryHolder.Construct();
            _maxIndicator.Construct(_inventoryHolder.Inventory);
            _handsAnimator.Construct(_inventoryHolder.Inventory);
            _storage.Construct(_inventoryHolder.Inventory);
        }
    }
}