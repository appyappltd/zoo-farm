using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Logic.Animals;
using UnityEngine;

namespace Logic.LevelGoals
{
    [CreateAssetMenu(menuName = "Presets/GoalPreset", fileName = "NewGoalPreset", order = 0)]
    public class GoalPreset : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<AnimalType, int> _animalsToRelease;
        [SerializeField] [Min(0)] private int _cashRewardForCompletingGoal;

        public IReadOnlyDictionary<AnimalType, int> AnimalsToRelease => _animalsToRelease;
        public int CashRewardForCompletingGoal => _cashRewardForCompletingGoal;
    }
}