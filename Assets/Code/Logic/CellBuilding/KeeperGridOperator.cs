namespace Logic.CellBuilding
{
    public class KeeperGridOperator : BuildGridOperator
    {
        protected override void BuildCell(BuildPlaceMarker marker)
        {
            GameFactory.CreateKeeper(marker.BuildPosition);
        }
    }
}