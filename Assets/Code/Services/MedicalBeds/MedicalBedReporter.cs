using System;
using System.Collections.Generic;
using Data.ItemsData;
using Logic.Medical;
using Logic.Storages.Items;

namespace Services.MedicalBeds
{
    public class MedicalBedsReporter : IMedicalBedsReporter
    {
        private readonly List<MedicalBed> _medicalBeds = new List<MedicalBed>();
        private readonly List<MedicalToolId> _neededTools = new List<MedicalToolId>();

        public event Action Updated = () => { };

        public bool HasFreeBeds()
        {
            bool hasFree = false;

            for (var index = 0; index < _medicalBeds.Count; index++)
            {
                MedicalBed medicalBed = _medicalBeds[index];
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
                medicalBed.FeedUp -= OnFeedUp;
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
            medicalBed.FeedUp += OnFeedUp;
            Updated.Invoke();
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
            Updated.Invoke();
        }

        private void OnFeedUp() =>
            Updated.Invoke();
    }
}