using Data.ItemsData;
using Logic.Foods.FoodSettings;
using NaughtyAttributes;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "Static Data/Food Vendor Config", fileName = "NewFoodVendorConfig", order = 0)]
    public class FoodVendorConfig : ScriptableObject
    {
        [SerializeField] [ReadOnly] private string _foodName = "None";
        [SerializeField] private FoodItemData _handItemData;
        [SerializeField] [MinMaxSlider(0f, 10f)] private Vector2 _produceDurationRange;
        [SerializeField] private int _maxStockQuantity;

        public FoodId FoodId => _handItemData.FoodId;
        public FoodItemData HandItemData => _handItemData;
        public Vector2 ProduceDurationRange => _produceDurationRange;
        public int MaxStockQuantity => _maxStockQuantity;

        private void OnValidate()
        {
            if (_handItemData != null)
            {
                _foodName = _handItemData.FoodId.ToString();
            }
        }
    }
}