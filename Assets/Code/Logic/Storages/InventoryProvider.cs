using UnityEngine;

namespace Logic.Storages
{
    public class InventoryProvider : MonoBehaviour
    {
        private IInventory _inventory;

        public IInventory Inventory => _inventory;
    }
}