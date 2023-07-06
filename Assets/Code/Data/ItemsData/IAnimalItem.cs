using Logic.Storages.Items;

namespace Data.ItemsData
{
    public interface IAnimalItem : IItem
    {
        AnimalItemData AnimalItemData { get; }
    }
}