using UnityEngine;

namespace Logic.Storages
{
    public class InventoryHolder : MonoBehaviour, IInventoryProvider
    {
        [SerializeField] private int _maxWeight;

        private IInventory _inventory;
        
        public IInventory Inventory => _inventory;

        public IGetItem ItemGetter => _inventory;
        public IAddItem ItemAdder => _inventory;

        public void Construct()
        {
            _inventory = new Inventory(_maxWeight);
        }
    }
}