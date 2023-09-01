using Logic.Storages.Items;

namespace Logic.Storages
{
    public interface IGetItem : IGetItemObserver
    {
        public IItem Get();
        public bool TryPeek(ItemFilter filter, out IItem item);
        bool TryGet(ItemFilter filter, out IItem result);
    }
}