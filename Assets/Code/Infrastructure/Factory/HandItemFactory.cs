using Data.ItemsData;
using Infrastructure.AssetManagement;
using Logic.Animals;
using Logic.Foods.FoodSettings;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class HandItemFactory : IHandItemFactory
    {
        private readonly IAssetProvider _assets;

        public HandItemFactory(IAssetProvider assets)
        {
            _assets = assets;
        }

        public HandItem CreateAnimal(Vector3 at, Quaternion rotation, AnimalType animalType) =>
            _assets.Instantiate($"{AssetPath.HandItemPath}/{ItemId.Animal}/{animalType}", at, rotation)
                .GetComponent<HandItem>();

        public HandItem CreateFood(Vector3 at, Quaternion rotation, FoodId foodId) =>
            _assets.Instantiate($"{AssetPath.HandItemPath}/{ItemId.Food}/{foodId}", at, rotation)
                .GetComponent<HandItem>();
    }
}