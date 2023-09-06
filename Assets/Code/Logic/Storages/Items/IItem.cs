using Data.ItemsData;
using Logic.Movement;
using Logic.Translators;

namespace Logic.Storages.Items
{
    public interface IItem
    {
        int Weight { get; }
        ItemId ItemId { get; }
        IItemMover Mover { get; }
        IItemData ItemData { get; }
        public void Destroy();
    }
}