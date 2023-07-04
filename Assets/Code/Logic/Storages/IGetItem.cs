using System;
using Data.ItemsData;
using Logic.Storages.Items;

namespace Logic.Storages
{
    public interface IGetItem
    {
        public event Action<IItem> Removed;
        public IItem Get();
        public bool CanGet(ItemId byId, out IItem item);
    }
}