using Logic.AnimalsBehaviour;
using Logic.Medicine;
using NTC.Global.Cache;
using Services;
using Services.Camera;
using Tools;
using UnityEngine;

namespace Cutscene.Directors
{
    public class AnimalWalksToHouseCutscene : CutsceneDirector
    {
        [SerializeField] private MedicineBed _medicineBed;
        [SerializeField] private Transform _animal;
        [SerializeField] private Transform _plant;
        [SerializeField] [RequireInterface(typeof(ICutsceneTrigger))] private MonoBehaviour _animalIsHomeTrigger;

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
            CutsceneModules.Add(new CutsceneTimeAwaiter(0.2f, GlobalUpdate.Instance));
            CutsceneModules.Add(new CutsceneAction(() => _cameraOperatorService.Focus(_animal)));
            CutsceneModules.Add(new CutsceneTriggerAwaiter((ICutsceneTrigger) _animalIsHomeTrigger));
            CutsceneModules.Add(new CutsceneAction(() => _cameraOperatorService.Focus(_plant)));
            CutsceneModules.Add(new CutsceneTimeAwaiter(3f, GlobalUpdate.Instance));
            CutsceneModules.Add(new CutsceneAction(() => _cameraOperatorService.FocusOnDefault()));
            CutsceneModules.Add(new CutsceneAction(() => Destroy(gameObject)));
        }
    }
}