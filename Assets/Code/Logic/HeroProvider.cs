using Logic.Payment;
using Logic.Storages;
using UnityEngine;

namespace Logic
{
    public class HeroProvider : MonoBehaviour
    {
        [SerializeField] private int  _maxInventoryWeight;

        private IWallet _wallet;
        private IInventory _inventory;
        
        public IWallet Wallet => _wallet;
        public IInventory Inventory => _inventory;

        private void Awake()
        {
            _wallet = new Wallet();
            _inventory = new Inventory(_maxInventoryWeight);
        }
    }
}