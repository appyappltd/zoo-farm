using Logic.Foods.FoodSettings;

namespace Logic.CellBuilding
{
    public class RefrigeratorGridOperator : BuildGridOperator
    {
        protected override void BuildCell(BuildPlaceMarker marker)
        {
            GameFactory.CreateFoodVendor(marker.BuildPosition, marker.Location.Rotation, FoodId.Meat);
        }
    }
}