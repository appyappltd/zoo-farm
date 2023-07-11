using Infrastructure.Factory;
using Logic.Plants;
using Logic.Plants.PlantSettings;
using Logic.Spawners;
using Services.StaticData;
using StaticData;
using UnityEngine;

namespace Infrastructure.Builders
{
    public class GardenBadBuilder
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IPlantFactory _plantFactory;

        public GardenBadBuilder(IStaticDataService staticDataService, IPlantFactory plantFactory)
        {
            _staticDataService = staticDataService;
            _plantFactory = plantFactory;
        }
        
        public void Build(GameObject gardenBedObject, PlantId plantId)
        {
            GardenBed gardenBed = gardenBedObject.GetComponent<GardenBed>();
            HandItemSpawner handItemSpawner = gardenBedObject.GetComponent<HandItemSpawner>();

            GardenBedConfig gardenBedConfig = _staticDataService.GardenBedConfigById(plantId);
            gardenBed.Construct(gardenBedConfig, _plantFactory);
            handItemSpawner.Construct(gardenBedConfig.HandItemData);
        }
    }
}