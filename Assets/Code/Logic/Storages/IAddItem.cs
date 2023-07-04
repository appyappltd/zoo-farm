using System;
using Data.ItemsData;
using Logic.Storages.Items;

namespace Logic.Storages
{
    public interface IAddItem
    {
        public event Action<IItem> Added;
        public void Add(IItem item);
        public bool CanAdd(IItem item);
    }
}