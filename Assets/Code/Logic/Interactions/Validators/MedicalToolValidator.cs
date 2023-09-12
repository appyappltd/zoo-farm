using Logic.Medical;
using Services;
using Services.MedicalBeds;
using UnityEngine;

namespace Logic.Interactions.Validators
{
    public class MedicalToolValidator : MonoBehaviour, IInteractionValidator
    {
        [SerializeField] private MedicalToolStand _stand;

        private IMedicalBedsReporter _medicalBedsReporter;

        private void Awake() =>
            _medicalBedsReporter = AllServices.Container.Single<IMedicalBedsReporter>();

        public bool IsValid<T>(T target = default) =>
            _medicalBedsReporter.IsNeeds(_stand.ToolId);
    }
}