using Logic.Foods.FoodSettings;
using UnityEngine;

namespace Services.Food
{
    public interface IFoodVendor
    {
        FoodId Type { get; }
        bool IsReady { get; }
        Vector3 Position { get; }
    }
}