using NTC.Global.Cache;
using Services;
using Services.Camera;
using UnityEngine;

namespace Cutscene.Directors
{
    public class FirstHouseCutscene : CutsceneDirector
    {
        [SerializeField] private Transform _firstHouse;
        
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
            CutsceneModules.Add(new CutsceneAction(() => Debug.Log("Begin Catscene")));
            CutsceneModules.Add(new CutsceneAction(() => _cameraOperatorService.Focus(_firstHouse)));
            CutsceneModules.Add(new CutsceneTimeAwaiter(3f, GlobalUpdate.Instance));
            CutsceneModules.Add(new CutsceneAction(() => _cameraOperatorService.FocusOnDefault()));
        }
    }
}