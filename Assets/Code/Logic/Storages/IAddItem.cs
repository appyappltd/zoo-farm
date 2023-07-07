using Logic.Storages.Items;

namespace Logic.Storages
{
    public interface IAddItem : IAddItemObserver
    {
        public void Add(IItem item);
        public bool CanAdd(IItem item);
        bool TryAdd(IItem item);
    }
}