using Logic.Animals;
using Services.AnimalHouses;
using UnityEngine;

namespace Code.Infrastructure.Builders
{
    public class AnimalHouseBuilder
    {
        private readonly IAnimalHouseService _houseService;

        public AnimalHouseBuilder(IAnimalHouseService houseService) =>
            _houseService = houseService;

        public IAnimalHouse Build(GameObject houseObject)
        {
            IAnimalHouse house = houseObject.GetComponent<IAnimalHouse>();
            _houseService.RegisterHouse(house);
            return house;
        }
    }
}