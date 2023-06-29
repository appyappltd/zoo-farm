using Logic;
using Logic.Animals.AnimalsBehaviour;
using Logic.Medicine;
using NTC.Global.Cache;
using Services;
using Services.Camera;
using Tools;
using UnityEngine;

namespace Tutorial.Directors
{
    public class BeginnerTutorial : TutorialDirector
    {
        [SerializeField] private Transform _firstHouse;
        [SerializeField] private MedicineBed _medicineBed;
        [SerializeField] private Transform _animal;
        [SerializeField] private Transform _plant;
        [SerializeField] private Transform _animalTransmittingZone;
        [SerializeField] private Transform _healingOptions;
        [SerializeField] [RequireInterface(typeof(ITutorialTrigger))] private MonoBehaviour _animalIsHomeTrigger;
        // [SerializeField] [RequireInterface(typeof(ITutorialTrigger))] private MonoBehaviour _animalTaken;
        // [SerializeField] [RequireInterface(typeof(ITutorialTrigger))] private MonoBehaviour _animalOnBed;
        // [SerializeField] [RequireInterface(typeof(ITutorialTrigger))] private MonoBehaviour _healingOptionTaken;
        // [SerializeField] [RequireInterface(typeof(ITutorialTrigger))] private MonoBehaviour _animalHealed;
        [SerializeField] private TutorialArrow _arrow;

        private ICameraOperatorService _cameraOperatorService;

        private void Start()
        {
            _medicineBed.AnimalHealed += GetAnimal;
            Construct(AllServices.Container.Single<ICameraOperatorService>());
        }

        private void GetAnimal(Animal animal)
        {
            _animal.SetParent(animal.transform);
            _animal.localPosition = Vector3.zero;
            _medicineBed.AnimalHealed -= GetAnimal;
        }

        public void Construct(ICameraOperatorService cameraOperatorService)
        {
            _cameraOperatorService = cameraOperatorService;
        }
        
        protected override void CollectModules()
        {
            TutorialModules.Add(new TutorialAction(() => Debug.Log("Begin tutorial")));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _arrow.Move(_animalTransmittingZone.position);
                _cameraOperatorService.Focus(_animalTransmittingZone.position);
            }));
            TutorialModules.Add(new TutorialTimeAwaiter(1f, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.FocusOnDefault()));
            TutorialModules.Add(new TutorialTriggerAwaiter(_arrow));
            TutorialModules.Add(new TutorialTimeAwaiter(2f, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _arrow.Move(_medicineBed.transform.position);
                _cameraOperatorService.Focus(_medicineBed.transform.position);
            }));
            TutorialModules.Add(new TutorialTimeAwaiter(1f, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.FocusOnDefault()));
            TutorialModules.Add(new TutorialTriggerAwaiter(_arrow));
            TutorialModules.Add(new TutorialTimeAwaiter(2f, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _arrow.Move(_healingOptions.position)));
            TutorialModules.Add(new TutorialTriggerAwaiter(_arrow));
            TutorialModules.Add(new TutorialTimeAwaiter(2f, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _arrow.Move(_medicineBed.transform.position)));
            TutorialModules.Add(new TutorialTriggerAwaiter(_arrow));
            TutorialModules.Add(new TutorialTimeAwaiter(2f, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _arrow.Move(_firstHouse.position);
                _cameraOperatorService.Focus(_firstHouse);
            }));
            TutorialModules.Add(new TutorialTimeAwaiter(3f, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.FocusOnDefault()));
            TutorialModules.Add(new TutorialTriggerAwaiter(_arrow));
            TutorialModules.Add(new TutorialTimeAwaiter(2f, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _arrow.Hide()));
            TutorialModules.Add(new TutorialTimeAwaiter(0.2f, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.Focus(_animal)));
            TutorialModules.Add(new TutorialTriggerAwaiter((ITutorialTrigger) _animalIsHomeTrigger));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _arrow.Move(_plant.position);
                _cameraOperatorService.Focus(_plant);
            }));
            TutorialModules.Add(new TutorialTimeAwaiter(3f, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _cameraOperatorService.FocusOnDefault()));
            TutorialModules.Add(new TutorialTriggerAwaiter(_arrow));
            TutorialModules.Add(new TutorialTimeAwaiter(2f, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _arrow.Hide()));
            TutorialModules.Add(new TutorialAction(() => Destroy(gameObject)));
        }
    }
}