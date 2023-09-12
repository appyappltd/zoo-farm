using Data.ItemsData;
using Logic.Movement;

namespace Logic.Storages.Items
{
    public interface IItem : IComponent
    {
        int Weight { get; }
        ItemId ItemId { get; }
        IItemMover Mover { get; }
        IItemData ItemData { get; }
        public void Destroy();
    }
}