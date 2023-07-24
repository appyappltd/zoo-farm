using Data.ItemsData;
using Logic.Foods.FoodSettings;
using UnityEngine;
using UnityEngine.Serialization;

namespace StaticData
{
    [CreateAssetMenu(menuName = "Static Data/Garden Bed Config", fileName = "NewGardenBedConfig", order = 0)]
    public class GardenBedConfig : ScriptableObject
    {
        [FormerlySerializedAs("_plantId")] [SerializeField] private FoodId _foodId;
        [SerializeField] private FoodItemData _handItemData;
        [SerializeField] private GrowStage[] _growStages;

        public FoodId FoodId => _foodId;
        public FoodItemData HandItemData => _handItemData; 
        public GrowthPlan GetGrowthPlan() =>
            new GrowthPlan(_growStages);
    }
}