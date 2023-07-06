using System;

namespace Logic.CellBuilding
{
    public class MedToolsGridOperator : BuildGridOperator
    {
        protected override void BuildCell(BuildPlaceMarker marker)
        {
            if (marker is not MedToolMarker medToolMarker)
                throw new Exception("Build marker is not MedToolMarker");
            
            GameFactory.CreateMedToolStand(medToolMarker.Location.Position, medToolMarker.Location.Rotation,
                medToolMarker.ToolIdType);
        }
    }
}