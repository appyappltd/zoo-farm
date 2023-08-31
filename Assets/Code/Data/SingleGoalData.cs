using System;
using AYellowpaper.SerializedCollections;
using Logic.Animals;
using UnityEngine;

namespace Data
{
    [Serializable]
    public struct SingleGoalData
    {
        public SerializedDictionary<AnimalType, int> AnimalsToRelease;
        [Min(0)] public int CashRewardForCompletingGoal;
    }
}