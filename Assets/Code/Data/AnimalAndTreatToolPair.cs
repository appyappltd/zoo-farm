using System;
using Data.ItemsData;
using Logic.Medicine;
using UnityEngine;

namespace Data
{
    [Serializable]
    public struct AnimalAndTreatToolPair
    {
        [SerializeField] private AnimalItemStaticData _animalStaticData;
        [SerializeField] private MedicineToolId _treatTool;

        public AnimalItemStaticData AnimalStaticData => _animalStaticData;
        public MedicineToolId TreatTool => _treatTool;
    }
}