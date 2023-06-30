using Logic.Medicine;
using UnityEngine;

namespace Logic.CellBuilding
{
    public class MedToolMarker : BuildPlaceMarker
    {
        [SerializeField] private MedicineType _toolType;

        public MedicineType ToolType => _toolType;
    }
}