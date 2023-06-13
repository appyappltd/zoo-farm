using System.Collections.Generic;
using Infrastructure.AssetManagement;
using Services.PersistentProgress;
using Services.Randomizer;
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

        public GameFactory(IAssetProvider assets, IRandomService randomService,
            IPersistentProgressService persistentProgressService)
        {
            _assets = assets;
            _randomService = randomService;
            _persistentProgressService = persistentProgressService;
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
            return animal;
        }

        public GameObject CreateAnimalHouse(Vector3 at) =>
            InstantiateRegistered(AssetPath.AnimalHousePath, at);

        public GameObject CreateHouseCell(Vector3 at) =>
            _assets.Instantiate(AssetPath.HouseCellPath, at);

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