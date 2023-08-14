namespace Logic.CellBuilding
{
    public class BreedingHouseGridOperator : BuildGridOperator
    {
        protected override void BuildCell(BuildPlaceMarker marker)
        {
            GameFactory.CreateBreedingHouse(marker.BuildPosition, marker.Location.Rotation);
        }
    }
}