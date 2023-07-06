using System;
using Data.ItemsData;
using Logic.Storages.Items;

namespace Logic.Storages
{
    public interface IGetItem : IGetItemObserver
    {
        public IItem Get();
        public bool TryPeek(ItemId byId, out IItem item);
        bool TryGet(ItemId byId, out IItem result);
    }

    public interface IGetItemObserver
    {
        public event Action<IItem> Removed;
    }
}