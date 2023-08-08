using Logic.Foods.FoodSettings;
using UnityEngine;

namespace Logic.CellBuilding
{
    public class GardenBedGridOperator : BuildGridOperator
    {
        [SerializeField] private FoodId _foodId;

        protected override void BuildCell(BuildPlaceMarker marker)
        {
            GameFactory.CreateFoodVendor(marker.BuildPosition, marker.Location.Rotation, _foodId);
        }
    }
}