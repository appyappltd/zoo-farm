using Logic;
using NTC.Global.Cache;
using Services;
using Services.Camera;
using UnityEngine;

namespace Tutorial.Directors
{
    public class FirstHouseCutscene : CutsceneDirector
    {
        [SerializeField] private Transform _firstHouse;
        [SerializeField] private TutorialArrow _arrow;

        private ICameraOperatorService _cameraOperatorService;

        private void Start()
        {
            Construct(AllServices.Container.Single<ICameraOperatorService>());
        }

        public void Construct(ICameraOperatorService cameraOperatorService)
        {
            _cameraOperatorService = cameraOperatorService;
        }

        protected override void CollectModules()
        {
            CutsceneModules.Add(new CutsceneAction(() => Debug.Log("Begin tutorial")));
            CutsceneModules.Add(new CutsceneAction(() =>
            {
                _arrow.Move(_firstHouse.position);
                _cameraOperatorService.Focus(_firstHouse);
            }));
            CutsceneModules.Add(new CutsceneTimeAwaiter(3f, GlobalUpdate.Instance));
            CutsceneModules.Add(new CutsceneAction(() =>
            {
                _cameraOperatorService.FocusOnDefault();
            }));
            CutsceneModules.Add(new CutsceneTriggerAwaiter(_arrow));
            CutsceneModules.Add(new CutsceneTimeAwaiter(2f, GlobalUpdate.Instance));
            CutsceneModules.Add(new CutsceneAction(() => _arrow.Hide()));
            CutsceneModules.Add(new CutsceneAction(() => Destroy(gameObject)));
        }
    }
}