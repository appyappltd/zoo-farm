using Logic;
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
        [SerializeField] private Transform _firstHealingOption;
        [SerializeField] private TutorialTriggerStatic _firstMedBadSpawned;
        [SerializeField] private TutorialTriggerStatic _firstHealingOptionSpawned;
        [SerializeField] private TutorialTriggerStatic _firstAnimalSpawned;
        [SerializeField] private TutorialTriggerStatic _medBedInteracted;
        [SerializeField] private TutorialTriggerStatic _medToolInteracted;
        [SerializeField] private TutorialTriggerStatic _animalTakenInteracted;
        [SerializeField] private TutorialTriggerStatic _foodTaken;
        [SerializeField] private TutorialTriggerStatic _animalHouseInteracted;
        [SerializeField] [RequireInterface(typeof(ITutorialTrigger))] private MonoBehaviour _houseBuilt;
        [SerializeField] [RequireInterface(typeof(ITutorialTrigger))] private MonoBehaviour _plantBuilt;
        [SerializeField] private TutorialArrow _arrow;

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
             // _firstMedBadSpawned.TriggeredPayload -= OnMedBadSpawned;
             // _firstHealingOptionSpawned.TriggeredPayload -= OnMedToolSpawned;
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
            TutorialModules.Add(new TutorialTriggerAwaiter(_firstMedBadSpawned));
            TutorialModules.Add(new TutorialAction(() => _arrow.Move(_firstHealingOption.position)));
            TutorialModules.Add(new TutorialTriggerAwaiter(_firstHealingOptionSpawned));
            TutorialModules.Add(new TutorialAction(() => _arrow.Move(_animalTransmittingZone.position)));
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
            TutorialModules.Add(new TutorialTriggerAwaiter(_medBedInteracted));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _arrow.Move(_firstHouse.position);
                _cameraOperatorService.Focus(_firstHouse);
            }));
            TutorialModules.Add(new TutorialTimeAwaiter(3f, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.FocusOnDefault()));
            TutorialModules.Add(new TutorialTriggerAwaiter((ITutorialTrigger) _houseBuilt));
            TutorialModules.Add(new TutorialAction(() => _arrow.Hide()));
            TutorialModules.Add(new TutorialTimeAwaiter(0.2f, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.Focus(_animal)));
            TutorialModules.Add(new TutorialTimeAwaiter(15f, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() =>
            {
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
            TutorialModules.Add(new TutorialAction(() => Destroy(gameObject)));
        }
    }
}