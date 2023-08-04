using System;
using System.Collections.Generic;
using Data.ItemsData;
using Logic.Storages.Items;

namespace Logic.Storages
{
    public class Inventory : IInventory
    {
        private readonly int _maxWeight;
        private readonly List<IItem> _items = new List<IItem>();

        private int _weight;
        private IItem _cashedGetItem;
        private bool _isActive;

        public Inventory(int maxWeight, List<IItem> items = null)
        {
            _maxWeight = maxWeight;

            if (items is not null)
                _items = items;
        }

        public bool IsFull => Weight == _maxWeight;
        public bool IsEmpty => Weight == 0;
        public int MaxWeight => _maxWeight;
        public int Weight => _weight;
        public bool IsActive => _isActive;

        public event Action<IItem> Added = _ => { };
        public event Action<IItem> Removed = _ => { };

        public void Activate() =>
            _isActive = true;

        public void Deactivate() =>
            _isActive = false;

        public void Add(IItem item)
        {
            _items.Add(item);
            _weight += item.Weight;
            Added.Invoke(item);
        }

        public bool CanAdd(IItem item) =>
            item.Weight + _weight <= _maxWeight;

        public IItem Get()
        {
            IItem result = _cashedGetItem;
            _cashedGetItem = null;
            _items.Remove(result);
            _weight -= result.Weight;
            Removed.Invoke(result);
            return result;
        }

        public bool TryPeek(ItemId byId, out IItem item)
        {
            item = null;

            if (_weight <= 0)
                return false;

            _cashedGetItem = _items.FindLast(found => (found.ItemId & byId) > 0);

            if (_cashedGetItem is null)
                return false;

            item = _cashedGetItem;
            return true;
        }

        public bool TryAdd(IItem item)
        {
            if (CanAdd(item))
            {
                Add(item);
                return true;
            }

            return false;
        }

        public bool TryGet(ItemId byId, out IItem result)
        {
            result = null;

            if (TryPeek(byId, out _))
            {
                result = Get();
                return true;
            }

            return false;
        }
    }
}