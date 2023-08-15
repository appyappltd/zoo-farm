using Logic.Animals;
using Services;
using Services.AnimalHouses;
using Ui.Services;
using Ui.Windows;

namespace Logic.CellBuilding
{
    public class HouseGridOperator : BuildGridOperator
    {
        private BuildPlaceMarker _cashedMarker;
        private IAnimalHouseService _houseService;
        private IWindowService _windowsService;

        protected override void OnAwake()
        {
            _houseService = AllServices.Container.Single<IAnimalHouseService>();
            _windowsService = AllServices.Container.Single<IWindowService>();
        }

        protected override void BuildCell(BuildPlaceMarker marker)
        {
            _cashedMarker = marker;
            HouseBuildWindow window = _windowsService.Open(WindowId.BuildHouse).GetComponent<HouseBuildWindow>();
            window.SetOnChoseCallback(OnAnimalChosen);
            
        }

        private void OnAnimalChosen(AnimalType type)
        {
            GameFactory.CreateAnimalHouse(_cashedMarker.BuildPosition, _cashedMarker.Location.Rotation, type).GetComponent<AnimalHouse>();
        }
    }
}