using Infrastructure.AssetManagement;
using Logic.Plants.PlantSettings;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class PlantFactory : IPlantFactory
    {
        private readonly IAssetProvider _assets;

        public PlantFactory(IAssetProvider assets) =>
            _assets = assets;

        public GameObject CreatePlant(Vector3 at, Quaternion rotation, PlantId plantId, int growStage) =>
            _assets.Instantiate($"{AssetPath.PlantPath}/{plantId}/Stage{growStage}", at, rotation);
    }
}