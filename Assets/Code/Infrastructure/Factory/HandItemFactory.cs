using Data;
using Logic.Animals;
using Data.ItemsData;
using Infrastructure.AssetManagement;
using Infrastructure.Builders;
using Logic.Foods.FoodSettings;
using Logic.Medical;
using Logic.Storages.Items;
using Services.StaticData;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class HandItemFactory : IHandItemFactory
    {
        private readonly IAssetProvider _assets;
        
        private readonly HandItemBuilder _handItemBuilder;
        
        public HandItemFactory(IAssetProvider assets, IStaticDataService staticData)
        {
            _assets = assets;
            _handItemBuilder = new HandItemBuilder(staticData);
        }

        public HandItem CreateAnimal(Vector3 at, Quaternion rotation, AnimalType animalType,
            AnimalAndTreatToolPair pair)
        {
            GameObject itemObject =
                _assets.Instantiate($"{AssetPath.HandItemPath}/{ItemId.Animal}/{animalType}", at, rotation);
            return _handItemBuilder.Build(itemObject, pair);
        }

        public HandItem CreateFood(Vector3 at, Quaternion rotation, FoodId foodId)
        {
            GameObject itemObject =
                _assets.Instantiate($"{AssetPath.HandItemPath}/{ItemId.Food}/{foodId}", at, rotation);
            return _handItemBuilder.Build(itemObject, foodId);
        }

        public HandItem CreateMedicalToolItem(Vector3 at, Quaternion rotation, MedicalToolId toolId)
        {
            GameObject itemObject =
                _assets.Instantiate($"{AssetPath.HandItemPath}/{ItemId.Medical}/{toolId}", at, rotation);
            return _handItemBuilder.Build(itemObject, toolId);
        }

        public HandItem CreateBreedingCurrency(Vector3 at, Quaternion rotation)
        {
            GameObject itemObject =
                _assets.Instantiate($"{AssetPath.HandItemPath}/{ItemId.BreedingCurrency}", at, rotation);
            return _handItemBuilder.Build(itemObject, ItemId.BreedingCurrency);
        }
    }
}