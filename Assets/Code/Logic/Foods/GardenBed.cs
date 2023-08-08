using System;
using Infrastructure.Factory;
using Logic.Foods.FoodSettings;
using NaughtyAttributes;
using Services.Food;
using StaticData;
using UnityEngine;

namespace Logic.Foods
{
    [RequireComponent(typeof(TimerOperator))]
    public class GardenBed : MonoBehaviour, IFoodVendor
    {
        [SerializeField] private TimerOperator _timerOperator;
        [SerializeField] private Transform _spawnPlace;
        
        private GrowthPlan _growthPlan;
        private Vector3 _position;

        public event Action GrowUp = () => { };

        public FoodId Type { get; private set; }
        public bool IsReady { get; private set; }
        public Vector3 Position => _position;

        private void Awake()
        {
            _position = transform.position;
            _timerOperator ??= GetComponent<TimerOperator>();
        }

        public void Construct(GardenBedConfig config, IFoodFactory foodFactory)
        {
            _growthPlan = config.GetGrowthPlan();
            _growthPlan.Init(foodFactory, config.FoodId, _spawnPlace, transform);
            Type = config.FoodId;
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

            IsReady = true;
            GrowUp.Invoke();
        }

        public void PlantNew()
        {
            IsReady = false;
            _growthPlan.Reset();
            Grow();
        }
    }
}