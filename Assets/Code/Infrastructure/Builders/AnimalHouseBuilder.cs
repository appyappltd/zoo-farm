using Logic.Animals;
using Services.AnimalHouses;

namespace Infrastructure.Builders
{
    public class AnimalHouseBuilder
    {
        private readonly IAnimalHouseService _houseService;

        public AnimalHouseBuilder(IAnimalHouseService houseService) =>
            _houseService = houseService;

        public void Build(IAnimalHouse house) =>
            _houseService.RegisterHouse(house);
    }
}