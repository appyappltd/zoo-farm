using System;

namespace Logic.CellBuilding
{
    public class MedToolGridOperator : BuildGridOperator
    {
        protected override void BuildCell(BuildPlaceMarker marker)
        {
            if (marker is not MedToolMarker medToolMarker)
                throw new Exception("Build marker is not MedToolMarker");
            
            GameFactory.CreateMedToolStand(medToolMarker.BuildPosition, medToolMarker.Location.Rotation,
                medToolMarker.ToolId);
        }
    }
}