using System;
using System.Collections.Generic;
using Data.ItemsData;
using Logic.Storages.Items;

namespace Logic.Storages
{
    public class Inventory : IInventory
    {
        private readonly int _maxWeight;
        
        private int _weight;
        
        private readonly List<IItem> _items = new List<IItem>();
        private IItem _cashedGetItem;

        public Inventory(int maxWeight, List<IItem> items = null)
        {
            _maxWeight = maxWeight;
            
            if (items is not null)
                _items = items;
        }

        public bool IsFull => Weight == _maxWeight;
        public int MaxWeight => _maxWeight;
        public int Weight => _weight;

        public event Action<IItem> Added = c => { };
        public event Action<IItem> Removed = c => { };
        
        public void Add(IItem item)
        {
            _items.Add(item);
            _weight += item.Weight;
            Added.Invoke(item);
        }

        public bool CanAdd(IItem item)
        {
            return item.Weight + _weight <= _maxWeight;
        }

        public IItem Get()
        {
            IItem result = _cashedGetItem;
            _cashedGetItem = null;
            _items.Remove(result);
            _weight -= result.Weight;
            Removed.Invoke(result);
            return result;
        }

        public bool CanGet(ItemId byId, out IItem item)
        {
            item = null;
            
            if (_weight <= 0)
                return false;
            
            _cashedGetItem = _items.Find(found => found.ItemId == byId);

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

            if (CanGet(byId, out _))
            {
                result = Get();
                return true;
            }

            return false;
        }
    }
}
