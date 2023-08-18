using System;
using Data.ItemsData;
using Logic.Medical;
using Logic.Storages;
using Logic.Storages.Items;
using Services;
using Services.MedicalBeds;
using UnityEngine;

namespace Logic.Interactions.Validators
{
    public class MedicalBedValidator : MonoBehaviour, IInteractionValidator
    {
        [SerializeField] private MedicalBed _medicalBed;
        
        private IMedicalBedsReporter _medicalBedsReporter;

        private void Awake() =>
            _medicalBedsReporter = AllServices.Container.Single<IMedicalBedsReporter>();

        public bool IsValid(IInventory inventory = default)
        {
            if (inventory is null)
                throw new NullReferenceException(nameof(inventory));

            if (inventory.TryPeek(ItemId.Medical, out IItem tool) == false)
                return true;

            MedicalToolItemData toolId = (MedicalToolItemData) tool.ItemData;
            return _medicalBed.ThreatTool == toolId.MedicineToolId;
        }
    }
}