using System.Collections.Generic;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Data.ItemsData;
using Infrastructure.AssetManagement;
using Infrastructure.Builders;
using Logic.Foods.FoodSettings;
using Logic.LevelGoals;
using Logic.Medical;
using Logic.Spawners;
using Services.AnimalHouses;
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
        private readonly MedBedBuilder _medBedBuilder;
        private readonly AnimalHouseBuilder _animalHouseBuilder;
        private readonly ReleaseInteractionZoneBuilder _releaseInteractionZoneBuilder;

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
        public IFoodFactory FoodFactory { get; }
        public IEffectFactory EffectFactory { get; }
        public IHandItemFactory HandItemFactory { get; }

        public GameFactory(IAssetProvider assets, IRandomService randomService,
            IPersistentProgressService progressService, IStaticDataService staticDataService,
            IAnimalsService animalsService, IPoolService poolService, IMedicalBedsReporter medicalBedsReporter,
            IAnimalHouseService houseService)
        {
            _assets = assets;
            _poolService = poolService;
            _randomService = randomService;
            _progressService = progressService;

            FoodFactory = new FoodFactory(assets);
            HandItemFactory = new HandItemFactory(assets);
            EffectFactory = new EffectFactory(assets, staticDataService, poolService);

            _animalHouseBuilder = new AnimalHouseBuilder(houseService);
            _medBedBuilder = new MedBedBuilder(medicalBedsReporter);
            _medStandBuilder = new MedStandBuilder(staticDataService);
            _animalBuilder = new AnimalBuilder(staticDataService, animalsService);
            _releaseInteractionZoneBuilder = new ReleaseInteractionZoneBuilder(staticDataService);
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

        public GameObject CreateAnimal(AnimalItemStaticData animalData, Vector3 at, Quaternion rotation)
        {
            GameObject animal = InstantiateRegistered($"{AssetPath.AnimalPath}/{animalData.AnimalType}", at, rotation);
            _animalBuilder.Build(animal.GetComponent<Animal>(), animalData);
            return animal;
        }

        public GameObject CreateAnimalChild(Vector3 at, Quaternion rotation, AnimalType type) =>
            _assets.Instantiate($"{AssetPath.ChildAnimalPath}/{type}", at, rotation);

        public GameObject CreateBreedingHouse(Vector3 at, Quaternion rotation)
        {
            GameObject houseObject = InstantiateRegistered(AssetPath.BreedingHouse, at, rotation);
            _animalHouseBuilder.Build(houseObject.GetComponent<IAnimalHouse>());
            return houseObject;
        }

        public ReleaseInteractionProvider CreateReleaseInteraction(Vector3 at, Quaternion rotation, AnimalType withType)
        {
            GameObject zoneObject = _assets.Instantiate(AssetPath.ReleaseInteractionPath, at, rotation);
            return _releaseInteractionZoneBuilder.Build(zoneObject, withType);

        }

        public GameObject CreateAnimalHouse(Vector3 at, Quaternion rotation, AnimalType animalType)
        {
            GameObject houseObject = InstantiateRegistered($"{AssetPath.AnimalHousePath}/{animalType}House", at, rotation);
            _animalHouseBuilder.Build(houseObject.GetComponent<IAnimalHouse>());
            return houseObject;
        }

        public GameObject CreateBuildCell(Vector3 at, Quaternion rotation) =>
            _assets.Instantiate(AssetPath.BuildCellPath, at, rotation);

        public GameObject CreateVisual(VisualType visual, Quaternion quaternion) =>
            _assets.Instantiate($"{AssetPath.VisualsPath}/{visual}");

        public GameObject CreateCollectibleCoin() =>
            _assets.Instantiate(AssetPath.CollectableCoinPath);

        public GameObject CreateFoodVendor(Vector3 at, Quaternion rotation, FoodId foodId)
        {
            GameObject gardenBedObject = _assets.Instantiate($"{AssetPath.FoodVendorPath}/{foodId}Vendor", at, rotation);
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

        public GameObject CreateKeeper(Vector3 at) =>
            _assets.Instantiate(AssetPath.KeeperPath, at);

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