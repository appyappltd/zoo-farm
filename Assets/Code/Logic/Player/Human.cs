using Logic.Storages;
using UnityEngine;

namespace Logic.Player
{
    public abstract class Human : MonoBehaviour
    {
        [SerializeField] protected InventoryHolder InventoryHolder;
        [SerializeField] protected InventoryAnimatorObserver HandsAnimator;
        [SerializeField] protected Storage Storage;
        
        private IInventory _inventory;
        public IInventory Inventory => InventoryHolder.Inventory;

        protected void Init()
        {
            InventoryHolder.Construct();
            HandsAnimator.Construct(InventoryHolder.Inventory);
            Storage.Construct(InventoryHolder.Inventory);
        }
    }
}