using Logic.Medicine;
using UnityEngine;

namespace Data.ItemsData
{
    [CreateAssetMenu(menuName = "Item Data/MedToolItemData", fileName = "NewMedToolItemData", order = 0)]
    public class MedToolItemData : ScriptableObject,  IItemData
    {
        public ItemId ItemId { get; set; }
        public int Weight { get; set; }
        public MedicineTool MedicineTool;
    }
}