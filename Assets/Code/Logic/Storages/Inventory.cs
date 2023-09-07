using System;
using System.Collections.Generic;
using System.Linq;
using Data.ItemsData;
using Logic.Foods.FoodSettings;
using Logic.Storages.Items;
using Logic.Upgrades;

namespace Logic.Storages
{
    public class Inventory : IInventory, IImprovable
    {
        private const string ImproveWeightException = "Improve weight value must be grater then zero";
        
        private int _maxWeight;
        private readonly List<IItem> _items = new List<IItem>();

        private int _weight;
        private IItem _cashedGetItem;

        private bool _isActive;

        public Inventory(int maxWeight, List<IItem> items = null)
        {
            _maxWeight = maxWeight;
            _isActive = true;

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

        void IImprovable.Improve(int byValue)
        {
            if (byValue <= 0)
                throw new ArgumentOutOfRangeException(nameof(byValue), byValue, ImproveWeightException);

            _maxWeight += byValue;
        }

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

        public bool CanAdd(IItem item)
        {
            if (_isActive == false)
                return false;

            return item.Weight + _weight <= _maxWeight;
        }

        public IItem Get()
        {
            IItem result = _cashedGetItem ?? _items.First();
            _cashedGetItem = null;
            _items.Remove(result);
            _weight -= result.Weight;
            Removed.Invoke(result);
            return result;
        }

        public bool TryPeek(ItemFilter filter, out IItem item)
        {
            item = null;

            if (_isActive == false)
                return false;

            if (_weight <= 0)
                return false;
            
            _cashedGetItem = _items.FindLast(found =>
            {
                if (filter.Contains(found.ItemId) == false)
                    return false;
                
                if (found.ItemId.HasFlag(ItemId.Food))
                    return filter.FoodIdFilter == FoodId.All || filter.FoodIdFilter == ((FoodItemData) found.ItemData).FoodId;

                return true;

            });
                

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

        public bool TryGet(ItemFilter filter, out IItem result)
        {
            result = null;

            if (TryPeek(filter, out _))
            {
                result = Get();
                return true;
            }

            return false;
        }
    }
}