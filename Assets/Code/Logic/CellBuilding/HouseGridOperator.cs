using Services;
using Services.AnimalHouse;
using UnityEngine;

namespace Logic.CellBuilding
{
    public class HouseGridOperator : BuildGridOperator
    {
        private IAnimalHouseService _houseService;

        protected override void BuildCell(Vector3 at)
        {
            _houseService.BuildHouse(at);
        }

        protected override void OnAwake()
        {
            _houseService = AllServices.Container.Single<IAnimalHouseService>();
        }
    }
}