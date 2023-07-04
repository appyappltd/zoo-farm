using Logic.Movement;
using Logic.Storages.Items;
using UnityEngine;

namespace Data.ItemsData
{
    [RequireComponent(typeof(ItemMover))]
    public class HandItem : MonoBehaviour, IItem
    {
        [SerializeField] private int _weight;
        [SerializeField] private ItemId _itemId;
        
        private IItemMover _mover;

        public int Weight => _weight;
        public ItemId ItemId => _itemId;
        public IItemMover Mover => _mover;
        
        private void Awake() =>
            _mover = GetComponent<IItemMover>();
        
        public void Destroy() =>
            Destroy(gameObject);
    }
}
