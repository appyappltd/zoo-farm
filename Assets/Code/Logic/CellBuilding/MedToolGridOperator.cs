using System;

namespace Logic.CellBuilding
{
    public class MedToolGridOperator : BuildGridOperator
    {
        protected override void BuildCell(BuildPlaceMarker marker)
        {
            if (marker is not MedToolMarker medToolMarker)
                throw new Exception("Build marker is not MedToolMarker");
            
            GameFactory.CreateMedTool(medToolMarker.Location.Position, medToolMarker.Location.Rotation,
                medToolMarker.ToolType);
        }
    }
}