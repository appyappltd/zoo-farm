using Data.ItemsData;
using Logic.Plants.PlantSettings;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "Static Data/Garden Bed Config", fileName = "NewGardenBedConfig", order = 0)]
    public class GardenBedConfig : ScriptableObject
    {
        [SerializeField] private PlantId _plantId;
        [SerializeField] private PlantItemData _handItemData;
        [SerializeField] private GrowStage[] _growStages;

        public PlantId PlantId => _plantId;
        public PlantItemData HandItemData => _handItemData; 
        public GrowthPlan GetGrowthPlan() =>
            new GrowthPlan(_growStages);
    }
}