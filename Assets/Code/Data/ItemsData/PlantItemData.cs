using Logic.Plants.PlantSettings;
using UnityEngine;

namespace Data.ItemsData
{
    [CreateAssetMenu(menuName = "Item Data/Plant Item Data", fileName = "NewPlantItemData", order = 0)]
    public class PlantItemData : ScriptableObject, IItemData
    {
        [field: SerializeField] public ItemId ItemId { get; set; }
        [field: SerializeField] public int Weight { get; set; }

        public PlantId PlantId;
    }
}