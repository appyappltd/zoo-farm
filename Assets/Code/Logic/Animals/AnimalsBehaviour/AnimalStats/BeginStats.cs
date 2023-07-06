using System;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Logic.Animals.AnimalsBehaviour.AnimalStats
{
    [Serializable]
    public class BeginStats
    {
        [SerializeField] [MinMaxSlider(0, 100f)] private Vector2 VitalityRange;
        [SerializeField] [MinMaxSlider(0, 100f)] private Vector2 SatietyRange;
        [SerializeField] [MinMaxSlider(0, 100f)] private Vector2 PeppinessRange;
        [SerializeField] [MinMaxSlider(0, 30f)] private Vector2 AgeRange;

        private float _randomVitality;
        private float _randomSatiety;
        private float _randomPeppiness;
        private float _randomAge;

        public float RandomVitality => _randomVitality;

        public float RandomSatiety => _randomSatiety;

        public float RandomPeppiness => _randomPeppiness;

        public float RandomAge => _randomAge;

        public void Calculate()
        {
            _randomVitality = GetRandomFromRange(VitalityRange);
            _randomSatiety = GetRandomFromRange(SatietyRange);
            _randomPeppiness = GetRandomFromRange(PeppinessRange);
            _randomAge = GetRandomFromRange(AgeRange);
        }

        private float GetRandomFromRange(Vector2 range) =>
            Random.Range(range.x, range.y);
    }
}