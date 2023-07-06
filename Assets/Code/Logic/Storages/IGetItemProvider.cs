namespace Logic.Storages
{
    public interface IGetItemProvider
    {
        public IGetItemObserver GetItemObserver { get; }
    }
}