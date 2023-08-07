using Infrastructure.Factory;
using Logic.Foods;
using Logic.Foods.FoodSettings;
using Services.Food;
using Services.StaticData;
using StaticData;
using UnityEngine;

namespace Infrastructure.Builders
{
    public class GardenBadBuilder
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IFoodFactory _foodFactory;
        private readonly IFoodService _foodService;

        public GardenBadBuilder(IStaticDataService staticDataService, IFoodFactory foodFactory,
            IFoodService foodService)
        {
            _staticDataService = staticDataService;
            _foodFactory = foodFactory;
            _foodService = foodService;
        }
        
        public void Build(GameObject gardenBedObject, FoodId foodId)
        {
            GardenBed gardenBed = gardenBedObject.GetComponent<GardenBed>();
            GardenBedOperator gardenGreedOperator = gardenBedObject.GetComponent<GardenBedOperator>();

            GardenBedConfig gardenBedConfig = _staticDataService.GardenBedConfigById(foodId);
            gardenGreedOperator.Construct(gardenBedConfig.HandItemData);
            gardenBed.Construct(gardenBedConfig, _foodFactory);
            _foodService.Register(gardenBed);
        }
    }
}