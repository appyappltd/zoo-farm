namespace Logic.CellBuilding
{
    public class FeederGridOperator : BuildGridOperator
    {
        protected override void BuildCell(BuildPlaceMarker marker)
        {
            GameFactory.CreateFeederFoundation(marker.BuildPosition, marker.Location.Rotation);
        }
    }
}