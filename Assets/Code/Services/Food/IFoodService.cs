using Logic.Foods.FoodSettings;

namespace Services.Food
{
    public interface IFoodService : IService
    {
        void Register(IFoodVendor foodVendor);
        IFoodVendor GetReadyVendor(FoodId byFoodId);
    }
}