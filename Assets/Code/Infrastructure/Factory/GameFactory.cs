using System.Collections.Generic;
using Data.ItemsData;
using Infrastructure.AssetManagement;
using Infrastructure.Builders;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Logic.Foods.FoodSettings;
using Logic.Medical;
using Logic.Spawners;
using Services.Animals;
using Services.MedicalBeds;
using Services.PersistentProgress;
using Services.Pools;
using Services.Randomizer;
using Services.StaticData;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IRandomService _randomService;
        private readonly IPersistentProgressService _progressService;
        private readonly IPoolService _poolService;

        private readonly AnimalBuilder _animalBuilder;
        private readonly MedStandBuilder _medStandBuilder;
        private readonly GardenBadBuilder _gardenBedBuilder;
        private readonly MedBedBuilder _medBedBuilder;
        private readonly AnimalHouseBuilder _animalHouseBuilder;

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
        public IFoodFactory FoodFactory { get; }
        public IEffectFactory EffectFactory { get; }
        public IHandItemFactory HandItemFactory { get; }

        public GameFactory(IAssetProvider assets, IRandomService randomService,
            IPersistentProgressService progressService, IStaticDataService staticDataService,
            IAnimalsService animalsService, IPoolService poolService, IMedicalBedsReporter medicalBedsReporter)
        {
            _assets = assets;
            _poolService = poolService;
            _randomService = randomService;
            _progressService = progressService;

            FoodFactory = new FoodFactory(assets);
            HandItemFactory = new HandItemFactory(assets);
            EffectFactory = new EffectFactory(assets, staticDataService, poolService);

            _animalHouseBuilder = new AnimalHouseBuilder();
            _medBedBuilder = new MedBedBuilder(medicalBedsReporter);
            _medStandBuilder = new MedStandBuilder(staticDataService);
            _animalBuilder = new AnimalBuilder(staticDataService, animalsService);
            _gardenBedBuilder = new GardenBadBuilder(staticDataService, FoodFactory);
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
            _poolService.DestroyAllPools();
            EffectFactory.Cleanup();
        }

        public void WarmUp()
        {
            EffectFactory.WarmUp();
        }

        public GameObject CreateHero(Vector3 at) =>
            InstantiateRegistered(prefabPath: AssetPath.HeroPath, at);

        public GameObject CreateHud() =>
            _assets.Instantiate(AssetPath.HudPath);

        public GameObject CreateAnimal(AnimalItemData animalData, Vector3 at, Quaternion rotation)
        {
            GameObject animal = InstantiateRegistered($"{AssetPath.AnimalPath}/{animalData.Type}", at, rotation);
            _animalBuilder.Build(animal.GetComponent<Animal>(), animalData);
            return animal;
        }

        public GameObject CreateAnimalHouse(Vector3 at, Quaternion rotation, AnimalType animalType) =>
            InstantiateRegistered($"{AssetPath.AnimalHousePath}/{animalType}", at, rotation);

        public GameObject CreateBuildCell(Vector3 at, Quaternion rotation) =>
            _assets.Instantiate(AssetPath.BuildCellPath, at, rotation);

        public GameObject CreateVisual(VisualType visual, Quaternion quaternion) =>
            _assets.Instantiate($"{AssetPath.VisualsPath}/{visual}");

        public GameObject CreateCollectibleCoin() =>
            _assets.Instantiate(AssetPath.CollectableCoinPath);

        public GameObject CreateGardenBad(Vector3 at, Quaternion rotation, FoodId foodId)
        {
            GameObject gardenBedObject = _assets.Instantiate(AssetPath.GardenBed, at, rotation);
            _gardenBedBuilder.Build(gardenBedObject, foodId);
            return gardenBedObject;
        }

        public GameObject CreateMedBed(Vector3 at, Quaternion rotation)
        {
            MedicalBed medicalBed = InstantiateRegistered(AssetPath.MedBed, at, rotation)
                .GetComponent<MedicalBed>();
            _medBedBuilder.Build(medicalBed);
            return medicalBed.gameObject;
        }

        public GameObject CreateMedToolStand(Vector3 at, Quaternion rotation, MedicalToolId toolIdType)
        {
            GameObject medToolStandObject = _assets.Instantiate(AssetPath.MedToolStandPath, at, rotation);
            MedicalToolStand medicalToolStand = medToolStandObject.GetComponent<MedicalToolStand>();
            _medStandBuilder.Build(medicalToolStand, toolIdType);
            return medToolStandObject;
        }

        public GameObject CreateVolunteer(Vector3 at, Transform parent)
        {
            GameObject volunteerObject = _assets.Instantiate(AssetPath.VolunteerPath, at);
            volunteerObject.transform.SetParent(parent);
            return volunteerObject;
        }

        public GameObject CreateHandItem(Vector3 at, Quaternion rotation, ItemId itemId) =>
            _assets.Instantiate($"{AssetPath.HandItemPath}/{itemId}", at, rotation);

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