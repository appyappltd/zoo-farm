using Logic;
using NTC.Global.Cache;
using Services;
using Services.Camera;
using UnityEngine;

namespace Tutorial.Directors
{
    public class FirstHouseTutorial : TutorialDirector
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
            TutorialModules.Add(new TutorialAction(() => Debug.Log("Begin tutorial")));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _arrow.Move(_firstHouse.position);
                _cameraOperatorService.Focus(_firstHouse);
            }));
            TutorialModules.Add(new TutorialTimeAwaiter(3f, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() =>
            {
                _cameraOperatorService.FocusOnDefault();
            }));
            TutorialModules.Add(new TutorialTriggerAwaiter(_arrow));
            TutorialModules.Add(new TutorialTimeAwaiter(2f, GlobalUpdate.Instance));
            TutorialModules.Add(new TutorialAction(() => _arrow.Hide()));
            TutorialModules.Add(new TutorialAction(() => Destroy(gameObject)));
        }
    }
}