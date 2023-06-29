using Infrastructure.Factory;
using Logic.Animals;
using Services;
using Services.AnimalHouses;
using UnityEngine;

namespace Logic.CellBuilding
{
    public class HouseGridOperator : BuildGridOperator
    {
        private IAnimalHouseService _houseService;
        private IGameFactory _gameFactory;

        protected override void BuildCell(Vector3 at)
        {
            AnimalHouse house = _gameFactory.CreateAnimalHouse(at).GetComponent<AnimalHouse>();
            _houseService.RegisterHouse(house);
        }

        protected override void OnAwake()
        {
            _houseService = AllServices.Container.Single<IAnimalHouseService>();
            _gameFactory = AllServices.Container.Single<IGameFactory>();
        }
    }
}