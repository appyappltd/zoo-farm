namespace Logic.CellBuilding
{
    public class GardenBedCellOperator : BuildGridOperator
    {
        protected override void BuildCell(BuildPlaceMarker marker)
        {
            GameFactory.CreateGardenBad(marker.Location.Position, marker.Location.Rotation);
        }
    }
}