using Infrastructure.Factory;
using NTC.Global.System;
using UnityEngine;

namespace Logic.Plants.PlantSettings
{
    public class GrowthPlan
    {
        private readonly GrowStage[] _stages;

        private int _currentStageIndex = -1;
        private GameObject[] _stageObjects;

        public GrowthPlan(GrowStage[] stages)
        {
            _stages = stages;
        }

        public void Init(IPlantFactory plantFactory, PlantId configPlantId, Transform spawnPlace, Transform parent)
        {
            _stageObjects = new GameObject[_stages.Length];
            
            for (var index = 0; index < _stages.Length; index++)
            {
                _stageObjects[index] = plantFactory.CreatePlant(spawnPlace.position, spawnPlace.rotation, configPlantId, _stages[index].StageId);
                _stageObjects[index].transform.SetParent(parent,true);
                _stageObjects[index].Disable();
            }
            
            Reset();
        }

        private void Grow()
        {
            DisableCurrentStateGameObject();
            _currentStageIndex++;
            EnableCurrentStateGameObject();
        }
        
        public bool TryGrow(out GrowStage stage)
        {
            stage = null;

            if (_currentStageIndex >= _stages.Length - 1)
            {
                DisableCurrentStateGameObject();
                return false;
            }

            Grow();
            stage = _stages[_currentStageIndex];

            return true;
        }

        private void EnableCurrentStateGameObject() =>
            _stageObjects[_currentStageIndex].Enable();

        private void DisableCurrentStateGameObject()
        {
            if (_currentStageIndex >= 0)
            {
                _stageObjects[_currentStageIndex].Disable();
            }
        }

        public void Reset() =>
            _currentStageIndex = -1;
    }
}