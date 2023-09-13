using Data.AnimalCounter;
using Logic.Animals;
using Logic.CellBuilding;
using Logic.LevelGoals;
using Logic.NPC.Volunteers;
using NTC.Global.Cache;
using Services;
using Services.Animals;
using Services.Camera;
using Tutorial.StaticTriggers;
using Tutorial.TextTutorial;
using Tutorial.TimePresets;
using Ui.Elements;
using UnityEngine;

namespace Tutorial.Directors
{
    public class BeginnerTutorial : TutorialDirector
    {
        [Space] [Header("Transform References")]
        [SerializeField] private Transform _firstHouse;
        [SerializeField] private Transform _firstFeeder;
        [SerializeField] private Transform _animalTransmittingView;
        [SerializeField] private Transform _firstMedToolSpawnPoint;
        [SerializeField] private Transform _firstMedBedSpawnPoint;
        [SerializeField] private Transform _animalReleaser;
        [SerializeField] private Transform _keeper;
        
        [Space] [Header("Triggers")]
        [SerializeField] private TutorialTriggerScriptableObject _beginnerCoinsCollected;
        [SerializeField] private TutorialTriggerScriptableObject _firstMedBadSpawned;
        [SerializeField] private TutorialTriggerScriptableObject _firstHealingOptionSpawned;
        [SerializeField] private TutorialTriggerScriptableObject _firstAnimalSpawned;
        [SerializeField] private TutorialTriggerScriptableObject _medBedInteracted;
        [SerializeField] private TutorialTriggerScriptableObject _medToolInteracted;
        [SerializeField] private TutorialTriggerScriptableObject _animalTakenInteracted;
        [SerializeField] private TutorialTriggerScriptableObject _animalHealed;
        [SerializeField] private TutorialTriggerScriptableObject _bowlFull;
        [SerializeField] private TutorialTriggerScriptableObject _bowlEmpty;
        [SerializeField] private TutorialTriggerScriptableObject _animalReleased;
        [SerializeField] private TutorialTriggerScriptableObject _feederBuilt;
        [SerializeField] private TutorialTriggerScriptableObject _firstVolunteerSpawned;
        [SerializeField] private TutorialTriggerScriptableObject _foodVendorSpawned;
        [SerializeField] private TutorialTriggerScriptableObject _goalComplete;
        [SerializeField] private TutorialTriggerScriptableObject _breedingZoneSpawned;
        [SerializeField] private TutorialTriggerScriptableObject _breedingBegin;

        [Space] [Header("Grids")]
        [SerializeField] private MedBedGridOperator _medBedGridOperator;
        [SerializeField] private MedToolGridOperator _medToolGridOperator;
        [SerializeField] private FeederGridOperator _feederGridOperator;
        [SerializeField] private GardenBedGridOperator _gardenBedGridOperator;
        [SerializeField] private BreedingZoneGridOperator _breedingZoneGridOperator;
        [SerializeField] private KeeperGridOperator _keeperGridOperator;
        [SerializeField] private VolunteerSpawner _volunteerSpawner;
        
        [Space] [Header("Configs")]
        [SerializeField] private BeginnerTutorialTimeDelayPreset _timeDelay;
        [SerializeField] private TextSequence _tutorialTextSequence;

        [Space] [Header("Other")]
        [SerializeField] private TutorialArrow _arrow;
        [SerializeField] private TextSetter _textSetter;
        [SerializeField] private FadeOutPanel _textFadeOutPanel;
        [SerializeField] private LevelGoalView _levelGoalView;
        
        private int _textPointer;
        
        private ICameraOperatorService _cameraOperatorService;
        private IAnimalCounter _animalCounter;

        private TutorialInteractedTriggerContainer _healingOption;
        private TutorialInteractedTriggerContainer _medBed;
        
        private Transform _volunteerTransform;
        private Transform _animalTransform;
        private Transform _breedingZoneTransform;
        
        private TutorialTrigger _hasBreedingPair = new TutorialTrigger();

        private void Start()
        {
            _firstMedBadSpawned.TriggeredPayload += OnMedBadSpawned;
            _firstHealingOptionSpawned.TriggeredPayload += OnMedToolSpawned;
            _firstAnimalSpawned.TriggeredPayload += OnAnimalSpawned;
            _firstVolunteerSpawned.TriggeredPayload += OnVolunteerSpawned;
            _breedingZoneSpawned.TriggeredPayload += OnBreedingZoneSpawned;

            Construct(AllServices.Container.Single<ICameraOperatorService>(),
                AllServices.Container.Single<IAnimalsService>().AnimalCounter);
        }

        private void OnDestroy()
        {
            _firstMedBadSpawned.TriggeredPayload -= OnMedBadSpawned;
            _firstHealingOptionSpawned.TriggeredPayload -= OnMedToolSpawned;
            _firstAnimalSpawned.TriggeredPayload -= OnAnimalSpawned;
            _firstVolunteerSpawned.TriggeredPayload -= OnVolunteerSpawned;
            _breedingZoneSpawned.TriggeredPayload -= OnBreedingZoneSpawned;
        }

        public void Construct(ICameraOperatorService cameraOperatorService, IAnimalCounter animalCounter)
        {
            _cameraOperatorService = cameraOperatorService;
            _animalCounter = animalCounter;
        }

        protected override void CollectModules()
        {
            TutorialModules.Add(new TutorialAction(() =>
            {
                HideText();
                ShowNextText();
                Debug.Log("Begin tutorial");
            }));
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
                HideText();
                _volunteerSpawner.Spawn();
                _arrow.Hide();
            }));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.MedicalToolSpawnedToVolunteerFocus, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.Focus(_volunteerTransform)));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.VolunteerFocusToArrowMoveToInteractionZone, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() =>
            {
                ShowNextText();
                _arrow.Move(_animalTransmittingView.position);
            }));
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
                HideText();
                ShowNextText();
                _feederGridOperator.ShowNextBuildCell();
                _arrow.Move(_firstFeeder.position);
                _cameraOperatorService.Focus(_firstFeeder);
            }));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.HouseFocusToPlayerFocus, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.FocusOnDefault()));
            TutorialModules.Add(new TutorialTriggerAwaiter(_feederBuilt));
            TutorialModules.Add(new TutorialAction(() =>
            {
                HideText();
                _arrow.Hide();
            }));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.HouseBuiltToAnimalFocus, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.Focus(_animalTransform)));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.AnimalFocusToPlantFocus, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() =>
            {
                ShowNextText();
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
            TutorialModules.Add(new TutorialAction(() => _arrow.Move(_firstFeeder.position)));
            TutorialModules.Add(new TutorialTriggerAwaiter(_bowlFull));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _arrow.Hide();
                HideText();
            }));
            TutorialModules.Add(new TutorialTriggerAwaiter(_bowlEmpty));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.BowlEmptyToReleaserFocus, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() =>
            {
                ShowNextText();
                _cameraOperatorService.Focus(_animalReleaser);
                _arrow.Move(_animalReleaser.position);
            }));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.AnimalReleaserFocusToPlayerFocus, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.FocusOnDefault()));
            TutorialModules.Add(new TutorialTriggerAwaiter(_goalComplete));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.GoalCompleteToVolunteersSpawn, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() =>
            {
                HideText();
                ShowNextText();
                _volunteerSpawner.Spawn();
                _medBedGridOperator.ShowNextBuildCell();
                _medToolGridOperator.ShowNextBuildCell();
                _arrow.Move(_medBedGridOperator.BuildCellPosition);
                _cameraOperatorService.Focus(_volunteerTransform);
            }));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.ThirdVolunteerFocusToPlayerFocus, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(_cameraOperatorService.FocusOnDefault));
            TutorialModules.Add(new TutorialTriggerAwaiter(_firstMedBadSpawned));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _arrow.Hide();
                _volunteerSpawner.Spawn();
            }));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _cameraOperatorService.FocusOnDefault();

                void OnHasBreedingPair(AnimalType type, AnimalCountData data)
                {
                    if (data.ReleaseReady >= AnimalPair.PairCount)
                    {
                        _animalCounter.Updated -= OnHasBreedingPair;
                        _hasBreedingPair.Trigger();
                    }
                }

                _animalCounter.Updated += OnHasBreedingPair;
            }));
            TutorialModules.Add(new TutorialTriggerAwaiter(_hasBreedingPair));
            TutorialModules.Add(new TutorialAction(() =>
            {
                HideText();
                ShowNextText();
                _volunteerSpawner.Spawn();
                _volunteerSpawner.StartAutoSpawning();
                _breedingZoneGridOperator.ShowNextBuildCell();
                _cameraOperatorService.Focus(_breedingZoneGridOperator.BuildCellPosition);
                _arrow.Move(_breedingZoneGridOperator.BuildCellPosition);
            }));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.BreedingZoneFocusToPlayerFocus, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.FocusOnDefault()));
            TutorialModules.Add(new TutorialTriggerAwaiter(_breedingZoneSpawned));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.BreedingZoneSpawnedToArrowMove, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() =>
            {
                HideText();
                _arrow.Move(_breedingZoneTransform.position);
            }));
            TutorialModules.Add(new TutorialTriggerAwaiter(_breedingBegin));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.BreedingBeginToBreedingComplete, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() =>
            {
                ShowNextText();
                _arrow.Move(_animalReleaser.position);
                _cameraOperatorService.Focus(_animalReleaser.position);
            }));
            TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.AnimalReleaserFocusToPlayerFocus, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.FocusOnDefault()));
            TutorialModules.Add(new TutorialTriggerAwaiter(_goalComplete));
            TutorialModules.Add(new TutorialAction(() =>
            {
                HideText();
                ShowNextText();
                _arrow.Hide();
            }));
            TutorialModules.Add(new TutorialTimeAwaiter(4f, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(HideText));
            // TutorialModules.Add(new TutorialAction(() =>
            // {
            //     _cameraOperatorService.Focus(_keeper);
            //     _keeperGridOperator.ShowNextBuildCell();
            // }));
            // TutorialModules.Add(new TutorialTimeAwaiter(_timeDelay.KeeperGridFocusToPlayerFocus, GlobalUpdate.Instance));
            // TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.FocusOnDefault()));
            TutorialModules.Add(new TutorialTimeAwaiter(3f, GlobalUpdate.Instance));
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
        
        private void OnBreedingZoneSpawned(GameObject breedingZone)
        {
            Debug.Log(breedingZone);
            _breedingZoneTransform = breedingZone.transform;
        }

        private void ShowNextText() =>
            _textFadeOutPanel.Show();

        private void HideText() =>
            _textFadeOutPanel.Hide(SetNextText);

        private void SetNextText() =>
            _textSetter.SetText(_tutorialTextSequence.Next(ref _textPointer));
    }
}