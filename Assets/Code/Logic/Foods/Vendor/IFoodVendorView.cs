using System;
using Logic.Foods.FoodSettings;
using Logic.Storages;
using Logic.Storages.Items;
using UnityEngine;

namespace Logic.Foods.Vendor
{
    public interface IFoodVendorView : IGetItem
    {
        FoodId Type { get; }
        bool IsReady { get; }
        Vector3 Position { get; }
        int MaxFoodCount { get; }
        event Action<IItem> FoodProduced;
        event Action BeginProduceFood;
    }
}