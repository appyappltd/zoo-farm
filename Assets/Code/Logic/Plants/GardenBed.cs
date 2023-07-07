using System;
using Logic.Plants.PlantSettings;
using NaughtyAttributes;
using UnityEngine;

namespace Logic.Plants
{
    [RequireComponent(typeof(TimerOperator))]
    public class GardenBed : MonoBehaviour
    {
        [SerializeField] private TimerOperator _timerOperator;
        [SerializeField] private MeshFilter _plantMeshFilter;

        private GrowthPlan _growthPlan;

        public event Action GrowUp = () => { };

        private void Awake()
        {
            _timerOperator ??= GetComponent<TimerOperator>();
        }

        public void Construct(GrowthPlan growthPlan)
        {
            _growthPlan = growthPlan;
            Grow();
        }

        [Button("Grow", enabledMode: EButtonEnableMode.Playmode)]
        public void Grow()
        {
            if (_growthPlan.TryGetNextStage(out GrowStage stage))
            {
                _timerOperator.SetUp(stage.GrowTime, Grow);
                _timerOperator.Play();
                _plantMeshFilter.sharedMesh = stage.Mesh;
                return;
            }
            
            GrowUp.Invoke();
        }
    }
}
