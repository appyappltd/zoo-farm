namespace Logic.Storages
{
    public interface IInventory : IAddItem, IGetItem
    {
        bool IsFull { get; }
        int MaxWeight { get; }
        int Weight { get; }
        bool IsEmpty { get; }
        void Activate();
        void Deactivate();
    }
}