using System;

namespace Data.ItemsData
{
    [Flags]
    public enum ItemId
    {
        None = 0,
        Animal = 1 << 1,
        Medical = 1 << 2,
        Food = 1 << 3,
        BreedingCurrency = 1 << 4,
        All = ~None
    }
}
