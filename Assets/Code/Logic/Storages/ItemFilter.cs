using Data.ItemsData;
using Logic.Foods.FoodSettings;

namespace Logic.Storages
{
    public readonly struct ItemFilter
    {
        private const int FullMack = 1111111111;
        
        public readonly ItemId ItemIdFilter;
        public readonly FoodId FoodIdFilter;

        public ItemFilter(ItemId itemIdFilter, FoodId foodIdFilter)
        {
            ItemIdFilter = itemIdFilter;
            FoodIdFilter = foodIdFilter;
        }
        
        public ItemFilter(ItemId itemIdFilter)
        {
            ItemIdFilter = itemIdFilter;
            FoodIdFilter = FoodId.All;
        }
        
        public bool Contains(ItemId mainMask, FoodId subMask)
        {
            return (ItemIdFilter & mainMask) != 0 && FoodIdFilter == subMask;
        }
        
        public bool Contains(ItemId mainMask)
        {
            return (ItemIdFilter & mainMask) != 0;
        }
    }
}