using Logic.Animals;
using Services;
using Services.AnimalHouses;
using UnityEngine;

namespace Logic.CellBuilding
{
    public class HouseGridOperator : BuildGridOperator
    {
        private IAnimalHouseService _houseService;

        protected override void BuildCell(Vector3 at, Quaternion rotation)
        {
            AnimalHouse house = GameFactory.CreateAnimalHouse(at, rotation).GetComponent<AnimalHouse>();
            _houseService.RegisterHouse(house);
        }

        protected override void OnAwake()
        {
            _houseService = AllServices.Container.Single<IAnimalHouseService>();
        }
    }
}