namespace Logic.Storages
{
    public interface IAddItemProvider
    {
        public IAddItemObserver AddItemObserver { get; }
    }
}