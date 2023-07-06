using System.Collections.Generic;
using Builders;
using Infrastructure.AssetManagement;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Logic.Medicine;
using Logic.Spawners;
using Logic.Storages;
using Services.Animals;
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
        
        private readonly AnimalBuilder _animalBuilder;
        private readonly MedStandBuilder _medStandBuilder;

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();


        public GameFactory(IAssetProvider assets, IRandomService randomService,
            IPersistentProgressService persistentProgressService, IStaticDataService staticDataService, IAnimalsService animalsService)
        {
            _assets = assets;
            _randomService = randomService;
            _persistentProgressService = persistentProgressService;
            _animalBuilder = new AnimalBuilder(staticDataService, animalsService);
            _medStandBuilder = new MedStandBuilder(staticDataService);
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

        public GameObject CreateAnimal(AnimalType animalType, Vector3 at)
        {
            GameObject animal = InstantiateRegistered($"{AssetPath.AnimalPath}/{animalType}", at);
            _animalBuilder.Build(animal.GetComponent<Animal>());
            return animal;
        }

        public GameObject CreateAnimalHouse(Vector3 at, Quaternion rotation) =>
            InstantiateRegistered(AssetPath.AnimalHousePath, at, rotation);

        public GameObject CreateBuildCell(Vector3 at, Quaternion rotation) =>
            _assets.Instantiate(AssetPath.BuildCellPath, at, rotation);

        public GameObject CreateVisual(VisualType visual, Quaternion quaternion, Transform container) =>
            _assets.Instantiate($"{AssetPath.VisualsPath}/{visual}", container);

        public GameObject CreateCollectibleCoin(Transform container) =>
            _assets.Instantiate(AssetPath.CollectableCoinPath, container);

        public GameObject CreateGardenBad(Vector3 at, Quaternion rotation) =>
            _assets.Instantiate(AssetPath.GardenBed, at, rotation);

        public GameObject CreateMedBed(Vector3 at, Quaternion rotation) =>
            InstantiateRegistered(AssetPath.MedBed, at, rotation);

        public GameObject CreateMedToolStand(Vector3 at, Quaternion rotation, MedicineToolId toolIdType)
        {
            GameObject medToolStandObject = _assets.Instantiate(AssetPath.MedToolStandPath, at, rotation);
            MedToolStand medToolStand = medToolStandObject.GetComponent<MedToolStand>();
            _medStandBuilder.Build(medToolStand, toolIdType);
            return medToolStandObject;
        }

        public GameObject CreateMedToolItem(Vector3 at, Quaternion rotation, MedicineToolId toolIdType)
        {
            
            return _assets.Instantiate($"{AssetPath.MedToolItemPath}/{toolIdType}", at, rotation);
        }

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

        private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
        {
            GameObject gameObject = _assets.Instantiate(path: prefabPath, at: at);
            RegisterProgressWatchers(gameObject);

            return gameObject;
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 at, Quaternion rotation)
        {
            GameObject gameObject = _assets.Instantiate(path: prefabPath, at: at, rotation);
            RegisterProgressWatchers(gameObject);

            return gameObject;
        }

        #endregion
    }
}