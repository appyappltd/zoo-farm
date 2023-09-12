using System;
using Data.ItemsData;
using Logic.Medical;
using UnityEngine;

namespace Data
{
    [Serializable]
    public struct AnimalAndTreatToolPair
    {
        [SerializeField] private AnimalItemStaticData _animalStaticData;
        [SerializeField] private TreatToolId _treatTool;

        public AnimalItemStaticData AnimalStaticData => _animalStaticData;
        public TreatToolId TreatTool => _treatTool;
    }
}