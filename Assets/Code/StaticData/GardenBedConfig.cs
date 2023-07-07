using Data.ItemsData;
using Logic.Plants.PlantSettings;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "Static Data/Garden Bed Config", fileName = "NewGardenBedConfig", order = 0)]
    public class GardenBedConfig : ScriptableObject
    {
        public PlantId PlantId;
        public PlantItemData HandItemData;
        
        [SerializeField] private GrowStage[] _growStages;

        public GrowthPlan GetGrowthPlan() =>
            new GrowthPlan(_growStages);
    }
}