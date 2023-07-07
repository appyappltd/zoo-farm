using Logic.Plants.PlantSettings;
using UnityEngine;

namespace Logic.CellBuilding
{
    public class GardenBedCellOperator : BuildGridOperator
    {
        [SerializeField] private PlantId _plantId;

        protected override void BuildCell(BuildPlaceMarker marker)
        {
            GameFactory.CreateGardenBad(marker.Location.Position, marker.Location.Rotation, _plantId);
        }
    }
}