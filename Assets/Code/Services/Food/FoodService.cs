using System.Collections.Generic;
using Logic.Foods.FoodSettings;

namespace Services.Food
{
    public class FoodService : IFoodService
    {
        private readonly HashSet<IFoodVendor> _vendors = new HashSet<IFoodVendor>();

        public void Register(IFoodVendor foodVendor)
        {
            _vendors.Add(foodVendor);
        }

        public IFoodVendor GetReadyVendor(FoodId byFoodId)
        {
            foreach (var vendor in _vendors)
            {
                if (vendor.Type == byFoodId && vendor.IsReady)
                    return vendor;
            }

            return null;
        }
    }
}