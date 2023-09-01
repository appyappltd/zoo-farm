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
            using (ListPool<AnimalType>.Get(out List<AnimalType> list))
            {
                for (var index = 0; index < _goals.Count; index++)
                {
                    var singleGoal = _goals[index];
                    list.AddRange(singleGoal.AnimalsToRelease.Keys);
                }

                return list.Distinct();
            }
        }   
    }
}