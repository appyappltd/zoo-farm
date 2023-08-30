namespace Logic.CellBuilding
{
    public class HouseGridOperator : BuildGridOperator
    {
        protected override void BuildCell(BuildPlaceMarker marker) =>
            GameFactory.CreateHouseFoundation(marker.BuildPosition, marker.Location.Rotation);
    }
}