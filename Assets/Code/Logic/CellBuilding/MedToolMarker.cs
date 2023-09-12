using Logic.Medical;
using UnityEngine;
using UnityEngine.Serialization;

namespace Logic.CellBuilding
{
    public class MedToolMarker : BuildPlaceMarker
    {
        [FormerlySerializedAs("_toolType")] [SerializeField] private TreatToolId _toolId;

        public TreatToolId ToolId => _toolId;
    }
}