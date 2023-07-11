using System;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Logic.Plants.PlantSettings
{
    [Serializable]
    public class GrowStage
    {
        [MinMaxSlider(0.1f, 10f)]
        [SerializeField] private Vector2 _growTime;
        [SerializeField] private int _stageId;
        
        public float GrowTime => Random.Range(_growTime.x, _growTime.y);
        public int StageId => _stageId;
    }
}