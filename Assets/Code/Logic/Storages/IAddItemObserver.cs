using System;
using Logic.Storages.Items;

namespace Logic.Storages
{
    public interface IAddItemObserver
    {
        public event Action<IItem> Added;
    }
}