using System.Collections.Generic;
using Data.ItemsData;
using Infrastructure.AssetManagement;
using Infrastructure.Builders;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Logic.Medicine;
using Logic.Plants.PlantSettings;
using Logic.Spawners;
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
        private readonly GardenBadBuilder _gardenBedBuilder;
        private readonly MedBedBuilder _medBedBuilder;

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
        public IPlantFactory PlantFactory { get; }

        public GameFactory(IAssetProvider assets, IRandomService randomService,
            IPersistentProgressService persistentProgressService, IStaticDataService staticDataService, IAnimalsService animalsService)
        {
            _assets = assets;
            _randomService = randomService;
            _persistentProgressService = persistentProgressService;

            PlantFactory = new PlantFactory(assets);

            MedicalToolNeedsReporter medicineToolReporter = new MedicalToolNeedsReporter();
            
            _animalBuilder = new AnimalBuilder(staticDataService, animalsService);
            _medStandBuilder = new MedStandBuilder(staticDataService, medicineToolReporter);
            _medBedBuilder = new MedBedBuilder(medicineToolReporter);
            _gardenBedBuilder = new GardenBadBuilder(staticDataService, PlantFactory);
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

        public GameObject CreateAnimal(AnimalItemData animalData, Vector3 at)
        {
            GameObject animal = InstantiateRegistered($"{AssetPath.AnimalPath}/{animalData.Type}", at);
            _animalBuilder.Build(animal.GetComponent<Animal>(), animalData);
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

        public GameObject CreateGardenBad(Vector3 at, Quaternion rotation, PlantId plantId)
        {
            GameObject gardenBedObject = _assets.Instantiate(AssetPath.GardenBed, at, rotation);
            _gardenBedBuilder.Build(gardenBedObject, plantId);
            return gardenBedObject;
        }

        public GameObject CreateMedBed(Vector3 at, Quaternion rotation)
        {
            MedicalBed medicalBed = InstantiateRegistered(AssetPath.MedBed, at, rotation)
                .GetComponent<MedicalBed>();
            _medBedBuilder.Build(medicalBed);
            return medicalBed.gameObject;
        }

        public GameObject CreateMedToolStand(Vector3 at, Quaternion rotation, MedicineToolId toolIdType)
        {
            GameObject medToolStandObject = _assets.Instantiate(AssetPath.MedToolStandPath, at, rotation);
            MedToolStand medToolStand = medToolStandObject.GetComponent<MedToolStand>();
            _medStandBuilder.Build(medToolStand, toolIdType);
            return medToolStandObject;
        }

        public GameObject CreateMedToolItem(Vector3 at, Quaternion rotation, MedicineToolId toolIdType) =>
            _assets.Instantiate($"{AssetPath.MedToolItemPath}/{toolIdType}", at, rotation);

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