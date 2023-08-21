using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Logic.Animals;
using UnityEngine;

namespace Logic.LevelGoals
{
    [CreateAssetMenu(menuName = "Presets/GoalPreset", fileName = "NewGoalPreset", order = 0)]
    public class GoalPreset : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<AnimalType, int> _amountAnimalToRelease;
        [SerializeField] [Min(0)] private int _cashRewardForCompletingGoal;

        public IReadOnlyDictionary<AnimalType, int> AmountAnimalToRelease => _amountAnimalToRelease;
        public int CashRewardForCompletingGoal => _cashRewardForCompletingGoal;
    }
}