using System;
using Cutscene;
using Services;
using Services.AnimalHouse;
using UnityEngine;

namespace Logic.CellBuilding
{
    public class HouseGridOperator : BuildGridOperator , ICutsceneTrigger
    {
        private IAnimalHouseService _houseService;

        public event Action Triggered = () => { };

        protected override void BuildCell(Vector3 at)
        {
            _houseService.BuildHouse(at);
            Triggered.Invoke();
        }

        protected override void OnAwake()
        {
            _houseService = AllServices.Container.Single<IAnimalHouseService>();
        }
    }
}