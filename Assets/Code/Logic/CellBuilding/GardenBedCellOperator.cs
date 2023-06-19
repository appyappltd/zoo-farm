using UnityEngine;

namespace Logic.CellBuilding
{
    public class GardenBedCellOperator : BuildGridOperator
    {
        protected override void BuildCell(Vector3 position)
        {
            GameFactory.CreateGardenBad(position);
        }
    }
}