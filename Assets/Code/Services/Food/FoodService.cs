using System.Collections.Generic;
using JetBrains.Annotations;
using Logic.Foods.FoodSettings;
using Logic.Foods.Vendor;
using UnityEngine;

namespace Services.Food
{
    public class FoodService : IFoodService
    {
        private readonly HashSet<IFoodVendorView> _vendors = new HashSet<IFoodVendorView>();

        public void Register(IFoodVendorView foodVendor)
        {
            _vendors.Add(foodVendor);
        }

        [CanBeNull]
        public IFoodVendorView GetReadyVendor(FoodId byFoodId)
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