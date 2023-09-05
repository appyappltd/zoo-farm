using Logic.Foods.FoodSettings;
using NaughtyAttributes;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "Static Data/Food Vendor Config", fileName = "NewFoodVendorConfig", order = 0)]
    public class FoodVendorConfig : ScriptableObject
    {
        [SerializeField] private FoodId _foodId;
        [SerializeField] [MinMaxSlider(0f, 10f)] private Vector2 _produceDurationRange;
        [SerializeField] private int _maxStockQuantity;

        public FoodId FoodId => _foodId;
        public Vector2 ProduceDurationRange => _produceDurationRange;
        public int MaxStockQuantity => _maxStockQuantity;
    }
}