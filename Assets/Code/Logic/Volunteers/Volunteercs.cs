using Logic.Storages;
using UnityEngine;

namespace Logic.Volunteer
{
    public class Volunteer : MonoBehaviour, IInventoryProvider
    {
        private IInventory _inventory;
        public IInventory Inventory => _inventory;

        public bool IsFree => _inventory.Weight <= 0;
        public bool WithAnimal => _inventory.Weight > 0;
        
        private void Awake()
        {
            _inventory = new Inventory(3);
        }
    }
}