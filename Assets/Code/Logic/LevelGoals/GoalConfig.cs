using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Logic.Animals;
using UnityEngine;

namespace Logic.LevelGoals
{
    [CreateAssetMenu(menuName = "Presets/GoalPreset", fileName = "NewGoalPreset", order = 0)]
    public class GoalConfig : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<AnimalType, int> _animalsToRelease;
        [SerializeField] [Min(0)] private int _cashRewardForCompletingGoal;
        [SerializeField] private string _levelName;

        public IReadOnlyDictionary<AnimalType, int> AnimalsToRelease => _animalsToRelease;
        public int CashRewardForCompletingGoal => _cashRewardForCompletingGoal;
        public string LevelName => _levelName;
    }
}