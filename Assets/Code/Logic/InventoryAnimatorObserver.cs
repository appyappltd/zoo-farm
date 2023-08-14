using Logic.Storages;
using Logic.Storages.Items;
using UnityEngine;

namespace Logic
{
    [RequireComponent(typeof(Animator))]
    public class InventoryAnimatorObserver : MonoBehaviour
    {
        private const string HandsLayerName = "Hands";
        
        [SerializeField] private Animator _animator;
        
        private IInventory _inventory; 
        private int _handsLayer;

        public void Construct(IInventory inventory)
        {
            _inventory = inventory;
            _handsLayer = _animator.GetLayerIndex(HandsLayerName);
            
            _inventory.Added += OnInventoryUpdated;
            _inventory.Removed += OnInventoryUpdated;
        }

        private void OnInventoryUpdated(IItem _) =>
            ChangeHandsState();

        private void Awake() =>
            _handsLayer = _animator.GetLayerIndex(HandsLayerName);

        private void OnDestroy()
        {
            _inventory.Added -= OnInventoryUpdated;
            _inventory.Removed -= OnInventoryUpdated;
        }

        private void ChangeHandsState() =>
            _animator.SetLayerWeight(_handsLayer, _inventory.Weight > 0 ? 1 : 0);
    }
}
