using Logic.Medicine;
using UnityEngine;

namespace Data.ItemsData
{
    [CreateAssetMenu(menuName = "Item Data/MedToolItemData", fileName = "NewMedToolItemData", order = 0)]
    public class MedToolItemData : ScriptableObject,  IItemData
    {
        [field: SerializeField] public ItemId ItemId { get; set; }
        [field: SerializeField] public int Weight { get; set; }
        public MedicineToolId MedicineToolId;
    }
}