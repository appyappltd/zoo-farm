using System;
using System.Collections.Generic;
using Data.ItemsData;
using Logic.Animals;
using Logic.Medical;
using Logic.Storages.Items;
using UnityEngine;

namespace Services.MedicalBeds
{
    public class MedicalBedsReporter : IMedicalBedsReporter
    {
        private readonly List<MedicalBed> _medicalBeds = new List<MedicalBed>();
        private readonly List<MedicalToolId> _neededTools = new List<MedicalToolId>();

        public event Action<MedicalToolId> ToolNeeds = i => { };

        public bool HasFreeBeds()
        {
            bool hasFree = false;

            for (var index = 0; index < _medicalBeds.Count; index++)
            {
                MedicalBed medicalBed = _medicalBeds[index];
                Debug.Log(index + " " + medicalBed.IsFree);
                hasFree = hasFree || medicalBed.IsFree;
            }

            return hasFree;
        }

        public void Cleanup()
        {
            foreach (MedicalBed medicalBed in _medicalBeds)
            {
                medicalBed.Added -= OnItemAdd;
                medicalBed.Removed -= OnItemRemoved;
            }
            
            _medicalBeds.Clear();
        }

        public void Register(MedicalBed medicalBed)
        {
            if (_medicalBeds.Contains(medicalBed))
                return;
            
            _medicalBeds.Add(medicalBed);
            medicalBed.Added += OnItemAdd;
            medicalBed.Removed += OnItemRemoved;
        }

        public bool IsNeeds(MedicalToolId toolId) =>
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