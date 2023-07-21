using Logic.Medical;
using Logic.Storages;
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
        
        public bool IsValid(IInventory inventory = default) =>
            _medicalBedsReporter.IsNeeds(_stand.ToolId);
    }
}