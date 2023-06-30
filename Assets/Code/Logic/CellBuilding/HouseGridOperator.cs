using Logic.Animals;
using Services;
using Services.AnimalHouses;

namespace Logic.CellBuilding
{
    public class HouseGridOperator : BuildGridOperator
    {
        private IAnimalHouseService _houseService;

        protected override void BuildCell(BuildPlaceMarker marker)
        {
            AnimalHouse house = GameFactory.CreateAnimalHouse(marker.Location.Position, marker.Location.Rotation).GetComponent<AnimalHouse>();
            _houseService.RegisterHouse(house);
        }

        protected override void OnAwake()
        {
            _houseService = AllServices.Container.Single<IAnimalHouseService>();
        }
    }
}