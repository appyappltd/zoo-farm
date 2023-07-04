using System;
using System.Collections.Generic;
using Logic.Storages.Items;
using UnityEngine;

namespace Logic.Storages
{
    public class Storage : MonoBehaviour
    {
        private readonly List<IItem> _items = new List<IItem>(5);
        
        [SerializeField] private Transform[] _places;
        
        private IInventory _inventory;
        private int _topIndex;

        public Action<IItem> Replenished = i => { };
        
        public Transform TopPlace => _places[_topIndex];
        private IItem TopItem
        {
            get => _items[_topIndex];
            set => _items[_topIndex] = value;
        }

        private void OnEnable()
        {
            _inventory.Added += PlaceItem;
            _inventory.Removed += RevertItem;
        }

        private void OnDisable()
        {
            _inventory.Added -= PlaceItem;
            _inventory.Removed -= RevertItem;
        }
        
        private void PlaceItem(IItem item)
        {
            item.Mover.Move(TopPlace, TopPlace);
            TopItem = item;
            Replenished.Invoke(item);
        }

        private void RevertItem(IItem item)
        {
            int revertItemIndex = _items.IndexOf(item);
            Sort(revertItemIndex);
        }

        private void Sort(int fromIndex)
        {
            for (int i = fromIndex; i < _items.Count - 1; i++)
            {
                _items[i] = _items[i + 1];
                Transform finishParent = _places[i];
                _items[i].Mover.Move(finishParent, finishParent);
            }
        }
    }
}
