using System;
using Infrastructure.Factory;
using Logic.Foods.FoodSettings;
using NaughtyAttributes;
using StaticData;
using UnityEngine;

namespace Logic.Foods
{
    [RequireComponent(typeof(TimerOperator))]
    public class GardenBed : MonoBehaviour
    {
        [SerializeField] private TimerOperator _timerOperator;
        [SerializeField] private Transform _spawnPlace;
        
        private GrowthPlan _growthPlan;

        public event Action GrowUp = () => { };

        private void Awake()
        {
            _timerOperator ??= GetComponent<TimerOperator>();
        }

        public void Construct(GardenBedConfig config, IFoodFactory foodFactory)
        {
            _growthPlan = config.GetGrowthPlan();
            _growthPlan.Init(foodFactory, config.FoodId, _spawnPlace, transform);
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