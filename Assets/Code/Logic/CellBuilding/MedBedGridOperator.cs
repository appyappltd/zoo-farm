namespace Logic.CellBuilding
{
    public class MedBedGridOperator : BuildGridOperator
    {
        protected override void BuildCell(BuildPlaceMarker marker)
        {
            GameFactory.CreateMedBed(marker.Location.Position, marker.Location.Rotation);
        }
    }
}
