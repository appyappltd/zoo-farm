using Logic.Foods.FoodSettings;

namespace Services.Food
{
    public interface IFoodService : IService
    {
        void Register(IFoodVendorView foodVendor);
        IFoodVendorView GetReadyVendor(FoodId byFoodId);
    }
}