using Data.ItemsData;
using Logic.Foods.FoodSettings;
using UnityEngine;
using UnityEngine.Serialization;

namespace StaticData
{
    [CreateAssetMenu(menuName = "Static Data/Garden Bed Config", fileName = "NewGardenBedConfig", order = 0)]
    public class GardenBedConfig : ScriptableObject
    {
        [SerializeField] private FoodItemData _handItemData;
        [SerializeField] private GrowStage[] _growStages;

        public FoodId FoodId => _handItemData.FoodId;
        public FoodItemData HandItemData => _handItemData; 
        public GrowthPlan GetGrowthPlan() =>
            new GrowthPlan(_growStages);
    }
}