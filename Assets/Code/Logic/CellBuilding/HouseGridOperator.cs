using Logic.Animals;
using Services;
using Services.AnimalHouses;
using UnityEngine;

namespace Logic.CellBuilding
{
    public class HouseGridOperator : BuildGridOperator
    {
        [SerializeField] private MedBedGridOperator _medBedGridOperator;

        private BuildPlaceMarker _cashedMarker;
        private IAnimalHouseService _houseService;

        protected override void OnAwake() =>
            _houseService = AllServices.Container.Single<IAnimalHouseService>();

        protected override void BuildCell(BuildPlaceMarker marker)
        {
            _cashedMarker = marker;
            _medBedGridOperator.HealedAnimasReporter.GetHealedAnimalType(OnAnimalChosen);
        }

        private void OnAnimalChosen(AnimalId Id)
        {
            AnimalHouse house = GameFactory.CreateAnimalHouse(_cashedMarker.Location.Position, _cashedMarker.Location.Rotation, Id.Type).GetComponent<AnimalHouse>();
            _houseService.RegisterHouse(house);
        }
    }
}