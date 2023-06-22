using System.Collections.Generic;
using Builders;
using Infrastructure.AssetManagement;
using Logic.AnimalsBehaviour;
using Logic.Spawners;
using Services.PersistentProgress;
using Services.Randomizer;
using Services.StaticData;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IRandomService _randomService;
        private readonly IPersistentProgressService _persistentProgressService;

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        private AnimalBuilder _animalBuilder;
        
        public GameFactory(IAssetProvider assets, IRandomService randomService,
            IPersistentProgressService persistentProgressService, IStaticDataService staticDataService)
        {
            _assets = assets;
            _randomService = randomService;
            _persistentProgressService = persistentProgressService;
            _animalBuilder = new AnimalBuilder(staticDataService);
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        public void WarmUp() { }

        public GameObject CreateHero(Vector3 at)
        {
            GameObject hero = InstantiateRegistered(prefabPath: AssetPath.HeroPath, at);
            return hero;
        }

        public GameObject CreateHud() =>
            _assets.Instantiate(AssetPath.HudPath);

        private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
        {
            GameObject gameObject = _assets.Instantiate(path: prefabPath, at: at);
            RegisterProgressWatchers(gameObject);

            return gameObject;
        }

        public GameObject CreateAnimal(AnimalType animalType, Vector3 at)
        {
            GameObject animal = InstantiateRegistered($"{AssetPath.AnimalPath}/{animalType}", at);
            _animalBuilder.Build(animal.GetComponent<Animal>());
            return animal;
        }

        public GameObject CreateAnimalHouse(Vector3 at) =>
            InstantiateRegistered(AssetPath.AnimalHousePath, at);

        public GameObject CreateBuildCell(Vector3 at) =>
            _assets.Instantiate(AssetPath.BuildCellPath, at);

        public GameObject CreateVisual(VisualType visual, Quaternion quaternion, Transform container) =>
            _assets.Instantiate($"{AssetPath.VisualsPath}/{visual}", container);

        public GameObject CreateCollectibleCoin(Transform container) =>
            _assets.Instantiate(AssetPath.CollectableCoinPath, container);

        public GameObject CreateGardenBad(Vector3 at) =>
            _assets.Instantiate(AssetPath.GardenBad, at);

        private GameObject InstantiateRegistered(string prefabPath)
        {
            GameObject gameObject = _assets.Instantiate(path: prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        #region RegisterProgress

        private void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);

            ProgressReaders.Add(progressReader);
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }

        #endregion
    }
}