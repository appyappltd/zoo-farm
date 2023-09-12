using Logic.Medical;
using UnityEngine;

namespace Data.ItemsData
{
    [CreateAssetMenu(menuName = "Item Data/MedToolItemData", fileName = "NewMedToolItemData", order = 0)]
    public class MedicalToolItemData : ScriptableObject, IItemData
    {
        [field: SerializeField] public ItemId ItemId { get; private set; }
        [field: SerializeField] public int Weight { get; private set; }
        public TreatToolId MedicineToolId;
    }
}