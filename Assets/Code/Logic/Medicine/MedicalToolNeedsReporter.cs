using System;
using System.Collections.Generic;
using Data.ItemsData;
using Logic.Storages.Items;

namespace Logic.Medicine
{
    public class MedicalToolNeedsReporter
    {
        private readonly List<MedicalBed> _medicalBeds = new List<MedicalBed>();
        private readonly List<MedicineToolId> _neededTools = new List<MedicineToolId>();

        public event Action<MedicineToolId> ToolNeeds = i => { };
        
        ~MedicalToolNeedsReporter()
        {
            foreach (MedicalBed medicalBed in _medicalBeds)
            {
                medicalBed.Added -= OnItemAdd;
                medicalBed.Removed -= OnItemRemoved;
            }
        }

        public void RegisterMedicineBed(MedicalBed bed)
        {
            _medicalBeds.Add(bed);
            bed.Added += OnItemAdd;
            bed.Removed += OnItemRemoved;
        }

        public bool IsNeeds(MedicineToolId toolId) =>
            _neededTools.Contains(toolId);

        private void OnItemRemoved(IItem item)
        {
            if ((item.ItemId & ItemId.Animal) == 0)
                return;
            
            AnimalItemData animalData = (AnimalItemData) item.ItemData;
            _neededTools.Remove(animalData.TreatToolId);
        }

        private void OnItemAdd(IItem item)
        {
            if ((item.ItemId & ItemId.Animal) == 0)
                return;
            
            AnimalItemData animalData = (AnimalItemData) item.ItemData;
            _neededTools.Add(animalData.TreatToolId);
            ToolNeeds.Invoke(animalData.TreatToolId);
        }
    }
}