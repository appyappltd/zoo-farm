using System;
using Logic.Storages.Items;

namespace Logic.Storages
{
    public interface IGetItemObserver
    {
        public event Action<IItem> Removed;
    }
}