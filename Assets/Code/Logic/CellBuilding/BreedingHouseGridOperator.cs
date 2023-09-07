namespace Logic.CellBuilding
{
    public class BreedingHouseGridOperator : BuildGridOperator
    {
        protected override void BuildCell(BuildPlaceMarker marker)
        {
            GameFactory.CreateBreedingPlace(marker.BuildPosition, marker.Location.Rotation);
        }
    }
}