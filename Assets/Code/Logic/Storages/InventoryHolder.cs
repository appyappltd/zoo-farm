using UnityEngine;

namespace Logic.Storages
{
    public class InventoryHolder : MonoBehaviour, IInventoryProvider
    {
        [SerializeField] private int _maxWeight;

        private IInventory _inventory;

        public IGetItemObserver GetItemObserver => _inventory;
        public IAddItemObserver AddItemObserver => _inventory;
        public IInventory Inventory => _inventory;

        public void Construct()
        {
            _inventory = new Inventory(_maxWeight);
        }
    }
}