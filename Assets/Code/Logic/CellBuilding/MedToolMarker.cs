using Logic.Medical;
using UnityEngine;
using UnityEngine.Serialization;

namespace Logic.CellBuilding
{
    public class MedToolMarker : BuildPlaceMarker
    {
        [FormerlySerializedAs("_toolType")] [SerializeField] private MedicalToolId _toolId;

        public MedicalToolId ToolId => _toolId;
    }
}