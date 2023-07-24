using Infrastructure.AssetManagement;
using Logic.Foods.FoodSettings;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class FoodFactory : IFoodFactory
    {
        private readonly IAssetProvider _assets;

        public FoodFactory(IAssetProvider assets) =>
            _assets = assets;

        public GameObject CreateFood(Vector3 at, Quaternion rotation, FoodId foodId, int growStage) =>
            _assets.Instantiate($"{AssetPath.PlantPath}/{foodId}/Stage{growStage}", at, rotation);
    }
}