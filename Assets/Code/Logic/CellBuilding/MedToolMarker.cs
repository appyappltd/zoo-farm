using Logic.Medicine;
using UnityEngine;

namespace Logic.CellBuilding
{
    public class MedToolMarker : BuildPlaceMarker
    {
        [SerializeField] private MedicineTool _toolType;

        public MedicineTool ToolType => _toolType;
    }
}