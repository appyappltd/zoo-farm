using Logic.Movement;
using Logic.Storages.Items;
using UnityEngine;

namespace Data.ItemsData
{
    [RequireComponent(typeof(ItemMover))]
    public class HandItem : MonoBehaviour, IItem
    {
        private IItemData _itemData;
        private IItemMover _mover;

        public int Weight => ItemData.Weight;
        public ItemId ItemId => ItemData.ItemId;
        public IItemMover Mover => _mover;
        public IItemData ItemData => _itemData;

        private void Awake()
        {
            _mover = GetComponent<IItemMover>();
        }

        public void Destroy() =>
            Destroy(gameObject);

        public void Construct(IItemData itemData)
        {
            _itemData = itemData;
        }
    }
}
