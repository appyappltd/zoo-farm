namespace Logic.CellBuilding
{
    public class MedBedGridOperator : BuildGridOperator
    {
        protected override void BuildCell(BuildPlaceMarker marker)
        {
            GameFactory.CreateMedBed(marker.BuildPosition, marker.Location.Rotation);
        }
    }
}
