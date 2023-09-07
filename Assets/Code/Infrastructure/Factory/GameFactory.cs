using System.Collections.Generic;
using Logic.Interactions;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Data.ItemsData;
using Infrastructure.AssetManagement;
using Infrastructure.Builders;
using Logic.Foods.FoodSettings;
using Logic.Medical;
using Logic.Payment;
using Logic.Spawners;
using Services.AnimalHouses;
using Services.Animals;
using Services.MedicalBeds;
using Services.PersistentProgress;
using Services.Pools;
using Services.Randomizer;
using Services.StaticData;
using Ui;
using Ui.LevelGoalPanel;
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
        private readonly InteractionZoneBuilder _interactionZoneBuilder;
        private readonly AnimalGoalPanelBuilder _animalGoalPanelBuilder;
        private readonly IStaticDataService _staticDataService;

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
            _staticDataService = staticDataService;
            _assets = assets;
            _poolService = poolService;
            _randomService = randomService;
            _progressService = progressService;

            FoodFactory = new FoodFactory(assets);
            HandItemFactory = new HandItemFactory(assets, staticDataService);
            EffectFactory = new EffectFactory(assets, staticDataService, poolService);
            
            _animalHouseBuilder = new AnimalHouseBuilder(houseService);
            _medBedBuilder = new MedBedBuilder(medicalBedsReporter);
            _medStandBuilder = new MedStandBuilder(staticDataService);
            _animalBuilder = new AnimalBuilder(staticDataService, animalsService);
            _interactionZoneBuilder = new InteractionZoneBuilder(animalsService, staticDataService);
            _animalGoalPanelBuilder = new AnimalGoalPanelBuilder(staticDataService);
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

        public Animal CreateAnimal(AnimalItemStaticData animalData, Vector3 at, Quaternion rotation)
        {
            GameObject animal = InstantiateRegistered($"{AssetPath.AnimalPath}/{animalData.AnimalType}", at, rotation);
            return _animalBuilder.Build(animal, animalData);
        }

        public Animal CreateAnimal(IAnimal clone, Vector3 at, Quaternion rotation)
        {
            GameObject animal = InstantiateRegistered($"{AssetPath.AnimalPath}/{clone.AnimalId.Type}", at, rotation);
            AnimalItemStaticData animalData = _staticDataService.AnimalItemDataById(clone.AnimalId.Type);
            return _animalBuilder.Build(animal, animalData);
        }
        
        public GameObject CreateAnimalChild(Vector3 at, Quaternion rotation, AnimalType type) =>
            _assets.Instantiate($"{AssetPath.ChildAnimalPath}/{type}", at, rotation);

        public IAnimalHouse CreateBreedingPlace(Vector3 at, Quaternion rotation)
        {
            GameObject houseObject = InstantiateRegistered(AssetPath.BreedingHouse, at, rotation);
            return _animalHouseBuilder.Build(houseObject);
        }

        public ReleaseInteractionProvider CreateReleaseInteraction(Vector3 at, Quaternion rotation, AnimalType withType)
        {
            GameObject zoneObject = _assets.Instantiate(AssetPath.ReleaseInteractionPath, at, rotation);
            return _interactionZoneBuilder.Build<ReleaseInteractionProvider>(zoneObject, withType);
        }

        public ChoseInteractionProvider CreateChoseInteraction(Vector3 at, Quaternion rotation, AnimalType withType)
        {
            GameObject zoneObject = _assets.Instantiate(AssetPath.ChoseHouseInteractionPath, at, rotation);
            return _interactionZoneBuilder.Build<ChoseInteractionProvider>(zoneObject, withType);
        }
        
        public ChoseInteractionProvider CreateChoseInteraction(Vector3 at, Quaternion rotation, FoodId withType)
        {
            GameObject zoneObject = _assets.Instantiate(AssetPath.ChoseHouseInteractionPath, at, rotation);
            return _interactionZoneBuilder.Build<ChoseInteractionProvider>(zoneObject, withType);
        }

        public GoalAnimalPanelProvider CreateAnimalGoalPanel(Vector3 at, Quaternion rotation, Transform parent,
            KeyValuePair<AnimalType, int> countTypePair)
        {
            GameObject panelObject = _assets.Instantiate(AssetPath.AnimalGoalPanelPath, at, rotation);
            panelObject.transform.SetParent(parent, true);
            return _animalGoalPanelBuilder.Build(panelObject, countTypePair);
        }

        public GameObject CreateFeederFoundation(Vector3 at, Quaternion rotation) =>
            _assets.Instantiate(AssetPath.FeederFoundationPath, at, rotation);

        public GameObject CreateFeeder(Vector3 at, Quaternion rotation, FoodId withFoodId) =>
            _assets.Instantiate($"{AssetPath.FeederPath}/{withFoodId}Feeder", at, rotation);

        public GameObject CreateHouseFoundation(Vector3 at, Quaternion rotation) =>
            _assets.Instantiate(AssetPath.HouseFoundationPath, at, rotation);

        public IAnimalHouse CreateAnimalHouse(Vector3 at, Quaternion rotation, AnimalType animalType)
        {
            GameObject houseObject = InstantiateRegistered($"{AssetPath.AnimalHousePath}/{animalType}House", at, rotation);
            return _animalHouseBuilder.Build(houseObject);
        }

        public GameObject CreateBuildCell(Vector3 at, Quaternion rotation, ConsumeType consumeType) =>
            _assets.Instantiate($"{AssetPath.BuildCellPath}{consumeType}", at, rotation);

        public GameObject CreateVisual(VisualType visual, Quaternion quaternion) =>
            _assets.Instantiate($"{AssetPath.VisualsPath}/{visual}");

        public GameObject CreateCollectibleCoin() =>
            _assets.Instantiate(AssetPath.CollectableCoinPath);

        public GameObject CreateFoodVendor(Vector3 at, Quaternion rotation, FoodId foodId)
        {
            GameObject gardenBedObject = _assets.Instantiate($"{AssetPath.FoodVendorPath}/{foodId}Vendor", at, rotation);
            return gardenBedObject;
        }

        public MedicalBed CreateMedBed(Vector3 at, Quaternion rotation)
        {
            GameObject medicalBedObject = InstantiateRegistered(AssetPath.MedBed, at, rotation);
            return _medBedBuilder.Build(medicalBedObject);
        }

        public MedicalToolStand CreateMedToolStand(Vector3 at, Quaternion rotation, MedicalToolId toolIdType)
        {
            GameObject medToolStandObject = _assets.Instantiate(AssetPath.MedToolStandPath, at, rotation);
            return _medStandBuilder.Build(medToolStandObject, toolIdType);
        }

        public GameObject CreateVolunteer(Vector3 at, Transform parent)
        {
            GameObject volunteerObject = _assets.Instantiate(AssetPath.VolunteerPath, at);
            volunteerObject.transform.SetParent(parent);
            return volunteerObject;
        }

        public GameObject CreateKeeper(Vector3 at) =>
            _assets.Instantiate(AssetPath.KeeperPath, at);

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

        private GameObject InstantiateRegistered(string prefabPath)
        {
            GameObject gameObject = _assets.Instantiate(path: prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
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