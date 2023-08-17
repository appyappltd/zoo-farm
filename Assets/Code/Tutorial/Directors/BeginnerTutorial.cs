using Logic.CellBuilding;
using Logic.NPC.Volunteers;
using NTC.Global.Cache;
using Services;
using Services.Camera;
using Tutorial.StaticTriggers;
using Tutorial.TimePresets;
using UnityEngine;

namespace Tutorial.Directors
{
    public class BeginnerTutorial : TutorialDirector
    {
        [SerializeField] private Transform _firstHouse;
        [SerializeField] private Transform _animalTransmittingView;
        [SerializeField] private Transform _firstMedToolSpawnPoint;
        [SerializeField] private Transform _firstMedBedSpawnPoint;
        [SerializeField] private Transform _animalReleaser;
        [SerializeField] private Transform _keeper;
        [SerializeField] private TutorialTriggerStatic _beginnerCoinsCollected;
        [SerializeField] private TutorialTriggerStatic _firstMedBadSpawned;
        [SerializeField] private TutorialTriggerStatic _firstHealingOptionSpawned;
        [SerializeField] private TutorialTriggerStatic _firstAnimalSpawned;
        [SerializeField] private TutorialTriggerStatic _medBedInteracted;
        [SerializeField] private TutorialTriggerStatic _medToolInteracted;
        [SerializeField] private TutorialTriggerStatic _animalTakenInteracted;
        [SerializeField] private TutorialTriggerStatic _animalHealed;
        [SerializeField] private TutorialTriggerStatic _bowlFull;
        [SerializeField] private TutorialTriggerStatic _bowlEmpty;
        [SerializeField] private TutorialTriggerStatic _animalReleased;
        [SerializeField] private TutorialTriggerStatic _houseBuilt;
        [SerializeField] private TutorialTriggerStatic _firstVolunteerSpawned;
        [SerializeField] private TutorialTriggerStatic _foodVendorSpawned;
        [SerializeField] private TutorialTriggerStatic _animalHouseSpawned;
        [SerializeField] private TutorialArrow _arrow;
        [SerializeField] private MedBedGridOperator _medBedGridOperator;
        [SerializeField] private MedToolGridOperator _medToolGridOperator;
        [SerializeField] private HouseGridOperator _houseGridOperator;
        [SerializeField] private GardenBedGridOperator _gardenBedGridOperator;
        [SerializeField] private KeeperGridOperator _keeperGridOperator;
        [SerializeField] private VolunteerSpawner _volunteerSpawner;
        [SerializeField] private BeginnerTutorialTimeDelayPreset _timeDelay;

        private ICameraOperatorService _cameraOperatorService;
        private TutorialInteractedTriggerContainer _healingOption;
        private TutorialInteractedTriggerContainer _medBed;
        private Transform _volunteerTransform;
        private Transform _animalTransform;

        private void Start()
        {
            _firstMedBadSpawned.TriggeredPayload += OnMedBadSpawned;
            _firstHealingOptionSpawned.TriggeredPayload += OnMedToolSpawned;
            _firstAnimalSpawned.TriggeredPayload += OnAnimalSpawned;
            _firstVolunteerSpawned.TriggeredPayload += OnVolunteerSpawned;

            Construct(AllServices.Container.Single<ICameraOperatorService>());
        }

        private void OnDestroy()
        {
            _firstMedBadSpawned.TriggeredPayload -= OnMedBadSpawned;
            _firstHealingOptionSpawned.TriggeredPayload -= OnMedToolSpawned;
            _firstAnimalSpawned.TriggeredPayload -= OnAnimalSpawned;
            _firstVolunteerSpawned.TriggeredPayload -= OnVolunteerSpawned;
        }

        public void Construct(ICameraOperatorService cameraOperatorService)
        {
            _cameraOperatorService = cameraOperatorService;
        }

        protected override void CollectModules()
        {
            TutorialModules.Add(new TutorialAction(() => Debug.Log("Begin tutorial")));
            TutorialModules.Add(new TutorialTriggerAwaiter(_beginnerCoinsCollected));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _medBedGridOperator.ShowNextBuildCell();
                _arrow.Move(_firstMedBedSpawnPoint.position);
            }));
            TutorialModules.Add(new TutorialTriggerAwaiter(_firstMedBadSpawned));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _medToolGridOperator.ShowNextBuildCell();
                _arrow.Move(_firstMedToolSpawnPoint.position);
            }));
            TutorialModules.Add(new TutorialTriggerAwaiter(_firstHealingOptionSpawned));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _volunteerSpawner.Spawn();
                _arrow.Hide();
            }));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.MedicalToolSpawnedToVolunteerFocus, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.Focus(_volunteerTransform)));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.VolunteerFocusToArrowMoveToInteractionZone, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _arrow.Move(_animalTransmittingView.position)));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.ArrowMoveToInteractionZoneToPlayerFocus, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.FocusOnDefault()));
            TutorialModules.Add(new TutorialTriggerAwaiter(_animalTakenInteracted));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _arrow.Move(_medBed.transform.position);
                _cameraOperatorService.Focus(_medBed.transform);
            }));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.MedicalBedFocusToPlayerFocus, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.FocusOnDefault()));
            TutorialModules.Add(new TutorialTriggerAwaiter(_medBedInteracted));
            TutorialModules.Add(new TutorialAction(() => _arrow.Move(_healingOption.transform.position)));
            TutorialModules.Add(new TutorialTriggerAwaiter(_medToolInteracted));
            TutorialModules.Add(new TutorialAction(() => _arrow.Move(_medBed.transform.position)));
            TutorialModules.Add(new TutorialTriggerAwaiter(_animalHealed));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _houseGridOperator.ShowNextBuildCell();
                _arrow.Move(_firstHouse.position);
                _cameraOperatorService.Focus(_firstHouse);
            }));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.HouseFocusToPlayerFocus, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.FocusOnDefault()));
            TutorialModules.Add(new TutorialTriggerAwaiter(_houseBuilt));
            TutorialModules.Add(new TutorialAction(() => _arrow.Hide()));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.HouseBuiltToAnimalFocus, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.Focus(_animalTransform)));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.AnimalFocusToPlantFocus, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() =>
            {
                ActivateAutoBuild(_gardenBedGridOperator);
                _gardenBedGridOperator.ShowNextBuildCell();
                _arrow.Move(_gardenBedGridOperator.BuildCellPosition);
                _cameraOperatorService.Focus(_gardenBedGridOperator.BuildCellPosition);
            }));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.PlantFocusToPlayerFocus, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.FocusOnDefault()));
            TutorialModules.Add(new TutorialTriggerAwaiter(_foodVendorSpawned));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.PlantBuiltToPlantBuilt, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _arrow.Move(_gardenBedGridOperator.BuildCellPosition)));
            TutorialModules.Add(new TutorialTriggerAwaiter(_foodVendorSpawned));
            TutorialModules.Add(new TutorialAction(() => _arrow.Move(_firstHouse.position)));
            TutorialModules.Add(new TutorialTriggerAwaiter(_bowlFull));
            TutorialModules.Add(new TutorialAction(() => _arrow.Hide()));
            TutorialModules.Add(new TutorialTriggerAwaiter(_bowlEmpty));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.BowlEmptyToReleaserFocus, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _cameraOperatorService.Focus(_animalReleaser);
                _arrow.Move(_animalReleaser.position);
            }));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.AnimalReleaserFocusToPlayerFocus, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.FocusOnDefault()));
            TutorialModules.Add(new TutorialTriggerAwaiter(_animalReleased));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _medBedGridOperator.ShowNextBuildCell();
                _arrow.Move(_medBedGridOperator.BuildCellPosition);
            }));
            TutorialModules.Add(new TutorialTriggerAwaiter(_firstMedBadSpawned));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _volunteerSpawner.StartAutoSpawning();
                _medToolGridOperator.ShowNextBuildCell();
                _houseGridOperator.ShowNextBuildCell();
                _arrow.Move(_medToolGridOperator.BuildCellPosition);
            }));
            TutorialModules.Add(new TutorialTriggerAwaiter(_firstHealingOptionSpawned));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _arrow.Hide();
                ActivateAutoBuild(_medBedGridOperator);
                ActivateAutoBuild(_medToolGridOperator);
                ActivateAutoBuild(_houseGridOperator);
            }));
            TutorialModules.Add(new TutorialTriggerAwaiter(_animalHouseSpawned));
            TutorialModules.Add(new TutorialTriggerAwaiter(_animalHouseSpawned));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _cameraOperatorService.Focus(_keeper);
                _keeperGridOperator.ShowNextBuildCell();
            }));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.KeeperGridFocusToPlayerFocus, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.FocusOnDefault()));
            TutorialModules.Add(new TutorialAction(() => Destroy(gameObject)));
        }

        private void ActivateAutoBuild(BuildGridOperator buildGrid)
        {
            buildGrid.SetAutoNext(true);
            buildGrid.ShowNextBuildCell();
        }

        private void OnAnimalSpawned(GameObject animalObject) =>
            _animalTransform = animalObject.transform;

        private void OnMedToolSpawned(GameObject toolObject) =>
            _healingOption = toolObject.GetComponent<TutorialInteractedTriggerContainer>();

        private void OnMedBadSpawned(GameObject bedObject) =>
            _medBed = bedObject.GetComponent<TutorialInteractedTriggerContainer>();

        private void OnVolunteerSpawned(GameObject volunteerObject) =>
            _volunteerTransform = volunteerObject.transform;
    }
}