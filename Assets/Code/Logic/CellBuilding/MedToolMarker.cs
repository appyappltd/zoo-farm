using Logic.Medicine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Logic.CellBuilding
{
    public class MedToolMarker : BuildPlaceMarker
    {
        [FormerlySerializedAs("_toolType")] [SerializeField] private MedicineToolId _toolId;

        public MedicineToolId ToolId => _toolId;
    }
}