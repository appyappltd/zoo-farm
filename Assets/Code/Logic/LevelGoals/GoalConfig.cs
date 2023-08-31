using System.Collections.Generic;
using System.Linq;
using Data;
using Logic.Animals;
using UnityEngine;
using UnityEngine.Pool;

namespace Logic.LevelGoals
{
    [CreateAssetMenu(menuName = "Presets/GoalPreset", fileName = "NewGoalPreset", order = 0)]
    public class GoalConfig : ScriptableObject
    {
        [SerializeField] private string _levelName;
        [SerializeField] private List<SingleGoalData> _goals;

        public string LevelName => _levelName;
        public IReadOnlyList<SingleGoalData> Goals => _goals;

        public IEnumerable<AnimalType> GetAnimalsToRelease()
        {
            List<AnimalType> _releaseTypes = ListPool<AnimalType>.Get();

            foreach (var singleGoal in _goals)
            {
                _releaseTypes.AddRange(singleGoal.AnimalsToRelease.Keys);
            }

            return _releaseTypes.Distinct();
        }   
    }
}