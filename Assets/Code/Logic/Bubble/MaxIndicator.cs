using Logic.Storages;
using Logic.Storages.Items;
using UnityEngine;

namespace Logic.Bubble
{
    public class MaxIndicator : MonoBehaviour
    {
        [SerializeField] private Bubble bubble;

        private IInventory _inventory;

        private void OnDestroy()
        {
            if (_inventory is null)
                return;

            _inventory.Added -= ChangeBubbleState;
            _inventory.Removed -= ChangeBubbleState;
        }
        
        public void Construct(IInventory inventory)
        {
            _inventory = inventory;
            _inventory.Added += ChangeBubbleState;
            _inventory.Removed += ChangeBubbleState;
            bubble.gameObject.SetActive(false);
        }
        
        private void ChangeBubbleState(IItem _) =>
            bubble.gameObject.SetActive(_inventory.IsFull);
    }
}