using System;
using Data.ItemsData;
using Logic.Medical;
using Logic.Player;
using Logic.Storages;
using Logic.Storages.Items;
using UnityEngine;

namespace Logic.Interactions.Validators
{
    public class MedicalBedValidator : MonoBehaviour, IInteractionValidator
    {
        [SerializeField] private MedicalBed _medicalBed;

        public bool IsValid<T>(T target = default)
        {
            if (target is not IHuman human)
                throw new ArgumentOutOfRangeException(nameof(target));

            if (human.Inventory.TryPeek(new ItemFilter(ItemId.Medical), out IItem tool) == false)
                return true;

            MedicalToolItemData toolId = (MedicalToolItemData) tool.ItemData;
            return _medicalBed.ThreatTool == toolId.MedicineToolId;
        }
    }
}