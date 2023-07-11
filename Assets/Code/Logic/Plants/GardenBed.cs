using System;
using Infrastructure.Factory;
using Logic.Plants.PlantSettings;
using NaughtyAttributes;
using StaticData;
using UnityEngine;

namespace Logic.Plants
{
    [RequireComponent(typeof(TimerOperator))]
    public class GardenBed : MonoBehaviour
    {
        [SerializeField] private TimerOperator _timerOperator;
        [SerializeField] private Transform _spawnPlace;
        
        private GrowthPlan _growthPlan;
        
        private GameObject[] _stageGameObjects; 

        public event Action GrowUp = () => { };

        private void Awake()
        {
            _timerOperator ??= GetComponent<TimerOperator>();
        }

        public void Construct(GardenBedConfig config, IPlantFactory plantFactory)
        {
            _growthPlan = config.GetGrowthPlan();
            _growthPlan.Init(plantFactory, config.PlantId, _spawnPlace, transform);
            PlantNew();
        }

        [Button("Grow", enabledMode: EButtonEnableMode.Playmode)]
        private void Grow()
        {
            if (_growthPlan.TryGrow(out GrowStage stage))
            {
                _timerOperator.SetUp(stage.GrowTime, Grow);
                _timerOperator.Play();

                return;
            }
            
            GrowUp.Invoke();
        }

        public void PlantNew()
        {
            _growthPlan.Reset();
            Grow();
        }
    }
}