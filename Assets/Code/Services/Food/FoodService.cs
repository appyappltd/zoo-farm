using System.Collections.Generic;
using JetBrains.Annotations;
using Logic.Foods.FoodSettings;
using UnityEngine;

namespace Services.Food
{
    public class FoodService : IFoodService
    {
        private readonly HashSet<IFoodVendor> _vendors = new HashSet<IFoodVendor>();

        public void Register(IFoodVendor foodVendor)
        {
            _vendors.Add(foodVendor);
        }

        [CanBeNull]
        public IFoodVendor GetReadyVendor(FoodId byFoodId)
        {
            foreach (var vendor in _vendors)
            {
                if (vendor.Type == byFoodId && vendor.IsReady)
                    return vendor;
            }

#if DEBUG
            Debug.LogWarning($"Food with {byFoodId} type does not exist");
#endif
            return null;
        }
    }
}