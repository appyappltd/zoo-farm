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
    [Serializable]
    [CreateAssetMenu(menuName = "Validators/Create MedicalBedValidatorSO", fileName = "MedicalBedValidatorSO", order = 0)]
    public class MedicalBedValidatorSO : ScriptableObject, IInteractionValidator
    {
        public MedicalBed _medicalBed;

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