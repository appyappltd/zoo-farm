using Logic.Animals;
using Logic.Medical;
using Services;
using Ui.Services;
using UnityEngine;

namespace Logic.CellBuilding
{
    public class MedBedGridOperator : BuildGridOperator
    {
#if UNITY_EDITOR
        [SerializeField] private bool _forceOpenBuildHouseWindow;
#endif

        private HealedAnimalsReporter _healedAnimalsReporter;
        public HealedAnimalsReporter HealedAnimasReporter => _healedAnimalsReporter;

        protected override void OnAwake()
        {
#if UNITY_EDITOR
            _healedAnimalsReporter = new HealedAnimalsReporter(AllServices.Container.Single<IWindowService>(),
                _forceOpenBuildHouseWindow);
#endif
            
#if !UNITY_EDITOR
            _healedAnimalsReporter = new HealedAnimalsReporter(AllServices.Container.Single<IWindowService>());
#endif
        }

        protected override void BuildCell(BuildPlaceMarker marker)
        {
            IMedicalBedReporter medicalBedReporter = GameFactory.CreateMedBed(marker.Location.Position, marker.Location.Rotation)
                .GetComponent<IMedicalBedReporter>();
            _healedAnimalsReporter.RegisterReporter(medicalBedReporter);
        }
    }
}