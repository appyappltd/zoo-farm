using UnityEngine;

namespace Logic.CellBuilding
{
    public class MedBedGridOperator : BuildGridOperator
    {
        protected override void BuildCell(Vector3 at, Quaternion rotation)
        {
            GameFactory.CreateMedBed(at, rotation);
        }
    }
}