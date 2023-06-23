using Logic.AnimalsBehaviour;
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
            Construct(AllServices.Container.Single<ICameraOperatorService>());

            _medicineBed.AnimalHealed += GetAnimal;
        }

        private void GetAnimal(Animal animal)
        {
            _animal = animal.transform;
            _medicineBed.AnimalHealed -= GetAnimal;
        }

        public void Construct(ICameraOperatorService cameraOperatorService)
        {
            _cameraOperatorService = cameraOperatorService;
        }
        
        protected override void CollectModules()
        {
            CutsceneModules.Add(new CutsceneAction(() => _cameraOperatorService.Focus(_animal)));
            CutsceneModules.Add(new CutsceneTriggerAwaiter((ICutsceneTrigger) _animalIsHomeTrigger));
            CutsceneModules.Add(new CutsceneAction(() => _cameraOperatorService.Focus(_plant)));
            CutsceneModules.Add(new CutsceneTimeAwaiter(3f, GlobalUpdate.Instance));
            CutsceneModules.Add(new CutsceneAction(() => _cameraOperatorService.FocusOnDefault()));
        }
    }
}