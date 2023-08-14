using NaughtyAttributes;
using UnityEngine;

namespace Logic.Storages
{
    public class InventoryHolder : MonoBehaviour, IInventoryProvider
    {
#if UNITY_EDITOR
        [SerializeField] [ReadOnly] private float _weight;
#endif
        
        [SerializeField] private int _maxWeight;

        private IInventory _inventory;
        
        public IInventory Inventory => _inventory;

        public IGetItem ItemGetter => _inventory;
        public IAddItem ItemAdder => _inventory;

        public void Construct()
        {
            _inventory = new Inventory(_maxWeight);
            
#if UNITY_EDITOR
            _inventory.Added += item => _weight += item.Weight;
            _inventory.Removed += item => _weight -= item.Weight;
#endif
        }
    }
}