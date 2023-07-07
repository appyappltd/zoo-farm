namespace Logic.Storages
{
    public interface IInventoryProvider : IGetItemProvider, IAddItemProvider
    {
        public IInventory Inventory { get; }
    }
}