using Logic.CellBuilding;
using Logic.Gates;
using Logic.Volunteers;
using NTC.Global.Cache;
using Services;
using Services.Camera;
using Tools;
using Tutorial.StaticTriggers;
using UnityEngine;

namespace Tutorial.Directors
{
    public class BeginnerTutorial : TutorialDirector
    {
        [SerializeField] private Transform _firstHouse;
        [SerializeField] private Transform _animal;
        [SerializeField] private Transform _plant;
        [SerializeField] private Transform _animalTransmittingZone;
        [SerializeField] private Transform _firstMedToolSpawnPoint;
        [SerializeField] private Transform _firstMedBedSpawnPoint;
        [SerializeField] private TutorialTriggerStatic _beginnerCoinsCollected;
        [SerializeField] private TutorialTriggerStatic _firstMedBadSpawned;
        [SerializeField] private TutorialTriggerStatic _firstHealingOptionSpawned;
        [SerializeField] private TutorialTriggerStatic _firstAnimalSpawned;
        [SerializeField] private TutorialTriggerStatic _medBedInteracted;
        [SerializeField] private TutorialTriggerStatic _medToolInteracted;
        [SerializeField] private TutorialTriggerStatic _animalTakenInteracted;
        [SerializeField] private TutorialTriggerStatic _animalHealed;
        [SerializeField] private TutorialTriggerStatic _foodTaken;
        [SerializeField] private TutorialTriggerStatic _animalHouseInteracted;
        [SerializeField] [RequireInterface(typeof(ITutorialTrigger))] private MonoBehaviour _houseBuilt;
        [SerializeField] [RequireInterface(typeof(ITutorialTrigger))] private MonoBehaviour _plantBuilt;
        [SerializeField] private TutorialArrow _arrow;
        [SerializeField] private MedBedGridOperator _medBedGridOperator;
        [SerializeField] private MedToolGridOperator _medToolGridOperator;
        [SerializeField] private HouseGridOperator _houseGridOperator;
        [SerializeField] private GardenBedGridOperator _gardenBedGridOperator;
        [SerializeField] private VolunteerSpawner _volunteerSpawner;
        [SerializeField] private Gate _toVolunteerGate;
        [SerializeField] private Gate _toYardGate;

        private ICameraOperatorService _cameraOperatorService;
        private TutorialInteractedTriggerContainer _healingOption;
        private TutorialInteractedTriggerContainer _medBed;

        private void Start()
        {
            _firstMedBadSpawned.TriggeredPayload += OnMedBadSpawned;
            _firstHealingOptionSpawned.TriggeredPayload += OnMedToolSpawned;
            _firstAnimalSpawned.TriggeredPayload += OnAnimalSpawned;
            Construct(AllServices.Container.Single<ICameraOperatorService>());
        }

        private void OnAnimalSpawned(GameObject animalObj)
        {
            _animal.SetParent(animalObj.transform, false);
        }

        private void OnDestroy()
         {
             _firstMedBadSpawned.TriggeredPayload -= OnMedBadSpawned;
             _firstHealingOptionSpawned.TriggeredPayload -= OnMedToolSpawned;
             _firstAnimalSpawned.TriggeredPayload -= OnAnimalSpawned;
         }

        private void OnMedToolSpawned(GameObject toolObject) =>
             _healingOption = toolObject.GetComponent<TutorialInteractedTriggerContainer>();
        
        private void OnMedBadSpawned(GameObject bedObject) =>
            _medBed = bedObject.GetComponent<TutorialInteractedTriggerContainer>();

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
                _volunteerSpawner.Spawn();
            }));
            TutorialModules.Add(new TutorialTriggerAwaiter(_firstHealingOptionSpawned));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _toVolunteerGate.Open();
                _arrow.Move(_animalTransmittingZone.position);
                _cameraOperatorService.Focus(_animalTransmittingZone);
            }));
            TutorialModules.Add(new TutorialTimeAwaiter(2f, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _cameraOperatorService.FocusOnDefault();
            }));
            TutorialModules.Add(new TutorialTriggerAwaiter(_animalTakenInteracted));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _arrow.Move(_medBed.transform.position);
                _cameraOperatorService.Focus(_medBed.transform.position);
            }));
            TutorialModules.Add(new TutorialTimeAwaiter(2f, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.FocusOnDefault()));
            TutorialModules.Add(new TutorialTriggerAwaiter(_medBedInteracted));
            TutorialModules.Add(new TutorialAction(() => _arrow.Move(_healingOption.transform.position)));
            TutorialModules.Add(new TutorialTriggerAwaiter(_medToolInteracted));
            TutorialModules.Add(new TutorialAction(() => _arrow.Move(_medBed.transform.position)));
            TutorialModules.Add(new TutorialTriggerAwaiter(_animalHealed));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _houseGridOperator.ShowNextBuildCell();
                _toYardGate.Open();
                _arrow.Move(_firstHouse.position);
                _cameraOperatorService.Focus(_firstHouse);
            }));
            TutorialModules.Add(new TutorialTimeAwaiter(3f, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.FocusOnDefault()));
            TutorialModules.Add(new TutorialTriggerAwaiter((ITutorialTrigger) _houseBuilt));
            TutorialModules.Add(new TutorialAction(() => _arrow.Hide()));
            TutorialModules.Add(new TutorialTimeAwaiter(0.2f, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.Focus(_animal)));
            TutorialModules.Add(new TutorialTimeAwaiter(1.5f, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _gardenBedGridOperator.ShowNextBuildCell();
                _arrow.Move(_plant.position);
                _cameraOperatorService.Focus(_plant);
            }));
            TutorialModules.Add(new TutorialTimeAwaiter(3f, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.FocusOnDefault()));
            TutorialModules.Add(new TutorialTriggerAwaiter((ITutorialTrigger) _plantBuilt));
            TutorialModules.Add(new TutorialAction(() => _arrow.Hide()));
            TutorialModules.Add(new TutorialTimeAwaiter(3f, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _arrow.Move(_plant.position)));
            TutorialModules.Add(new TutorialTriggerAwaiter(_foodTaken));
            TutorialModules.Add(new TutorialAction(() => _arrow.Move(_firstHouse.position)));
            TutorialModules.Add(new TutorialTriggerAwaiter(_animalHouseInteracted));
            TutorialModules.Add(new TutorialAction(() => _arrow.Hide()));
            TutorialModules.Add(new TutorialAction((() =>
            {
                _medBedGridOperator.SetAutoNext(true);
                _medBedGridOperator.ShowNextBuildCell();
                _medToolGridOperator.SetAutoNext(true);
                _medToolGridOperator.ShowNextBuildCell();
                _houseGridOperator.SetAutoNext(true);
                _houseGridOperator.ShowNextBuildCell();
                _gardenBedGridOperator.SetAutoNext(true);
                _gardenBedGridOperator.ShowNextBuildCell();
            })));
            TutorialModules.Add(new TutorialAction(() => Destroy(gameObject)));
        }
    }
}