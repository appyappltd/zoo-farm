namespace Logic.Storages
{
    public interface IInventory : IAddItem, IGetItem
    {
        bool IsFull { get; }
    }
}