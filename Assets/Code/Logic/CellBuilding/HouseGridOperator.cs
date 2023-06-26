using System;
using Cutscene;
using Infrastructure.Factory;
using Logic.Animals;
using Services;
using Services.AnimalHouses;
using UnityEngine;

namespace Logic.CellBuilding
{
    public class HouseGridOperator : BuildGridOperator , ICutsceneTrigger
    {
        private IAnimalHouseService _houseService;
        private IGameFactory _gameFactory;

        public event Action Triggered = () => { };

        protected override void BuildCell(Vector3 at)
        {
            AnimalHouse house = _gameFactory.CreateAnimalHouse(at).GetComponent<AnimalHouse>();
            _houseService.RegisterHouse(house);
            Triggered.Invoke();
        }

        protected override void OnAwake()
        {
            _houseService = AllServices.Container.Single<IAnimalHouseService>();
            _gameFactory = AllServices.Container.Single<IGameFactory>();
        }
    }
}