using Logic.Storages;
using Services;
using Services.MedicalBeds;
using UnityEngine;

namespace Logic.Interactions.Validators
{
    public class AnimalValidator : MonoBehaviour, IInteractionValidator
    {
        private IMedicalBedsReporter _medicalBedsReporter;

        private void Awake() =>
            _medicalBedsReporter = AllServices.Container.Single<IMedicalBedsReporter>();

        public bool IsValid(IInventory inventory = default) =>
            _medicalBedsReporter.HasFreeBeds();
    }
}