using System.Collections.Generic;
using Data;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "Static Data/Animals Sequence Config", fileName = "NewAnimalsSequenceConfig", order = 0)]
    public class AnimalsSequenceConfig : ScriptableObject
    {
        [SerializeField] private List<AnimalAndTreatToolPair> _pairs;

        public int PairsCount => _pairs.Count;
        
        public AnimalAndTreatToolPair GetPair(int byIndex) =>
            _pairs[byIndex];
    }
}