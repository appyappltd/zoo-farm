using Logic.Plants;
using Logic.Plants.PlantSettings;
using Logic.Spawners;
using Services.StaticData;
using StaticData;
using UnityEngine;

namespace Builders
{
    public class GardenBadBuilder
    {
        private readonly IStaticDataService _staticDataService;

        public GardenBadBuilder(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }
        
        public void Build(GameObject gardenBedObject, PlantId plantId)
        {
            GardenBed gardenBed = gardenBedObject.GetComponent<GardenBed>();
            HandItemSpawner handItemSpawner = gardenBedObject.GetComponent<HandItemSpawner>();

            GardenBedConfig gardenBedConfig = _staticDataService.GardenBedConfigById(plantId);
            gardenBed.Construct(gardenBedConfig.GetGrowthPlan());
            handItemSpawner.Construct(gardenBedConfig.HandItemData);
        }
    }
}