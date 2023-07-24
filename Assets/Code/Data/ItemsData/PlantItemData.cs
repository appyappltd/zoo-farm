using Logic.Foods.FoodSettings;
using UnityEngine;

namespace Data.ItemsData
{
    [CreateAssetMenu(menuName = "Item Data/Food Item Data", fileName = "NewFoodItemData", order = 0)]
    public class FoodItemData : ScriptableObject, IItemData
    {
        [field: SerializeField] public ItemId ItemId { get; set; }
        [field: SerializeField] public int Weight { get; set; }

        public FoodId FoodId;
    }
}