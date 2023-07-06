using Logic.Storages;
using Logic.Storages.Items;
using UnityEngine;

namespace Logic
{
    [RequireComponent(typeof(Animator))]
    public class HandsAnimator : MonoBehaviour
    {
        private IInventory _inventory; 
        private Animator animC;

        public void Construct(IInventory inventory)
        {
            _inventory = inventory;
            
            _inventory.Added += OnInventoryUpdated;
            _inventory.Removed += OnInventoryUpdated;
            
            animC = GetComponent<Animator>();
        }

        private void OnInventoryUpdated(IItem _) =>
            ChangeHandsState();

        private void OnDestroy()
        {
            _inventory.Added -= OnInventoryUpdated;
            _inventory.Removed -= OnInventoryUpdated;
        }

        private void ChangeHandsState()
        {
            animC.SetBool("Hold", _inventory.Weight > 0);
        }
    }
}
