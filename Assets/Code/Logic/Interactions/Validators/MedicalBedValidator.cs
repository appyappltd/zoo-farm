using System;
using Data.ItemsData;
using Logic.Storages;
using Logic.Storages.Items;
using Services;
using Services.MedicalBeds;
using UnityEngine;

namespace Logic.Interactions.Validators
{
    public class MedicalBedValidator : MonoBehaviour, IInteractionValidator
    {
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
            return _medicalBedsReporter.IsNeeds(toolId.MedicineToolId);
        }
    }
}