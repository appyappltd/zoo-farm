using Logic.Animals.AnimalFeeders;
using Logic.Foods.FoodSettings;

namespace Services.Feeders
{
    public interface IAnimalFeederService : IService
    {
        void Register(AnimalFeeder feeder);
        AnimalFeeder GetFeeder(FoodId byFoodId);
    }
}