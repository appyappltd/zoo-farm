using System;
using AYellowpaper;
using Infrastructure.Factory;
using Logic.Animals;
using Logic.Interactions;
using Logic.Spawners;
using Logic.TransformGrid;
using Services.Animals;
using Ui.LevelGoalPanel;
using UnityEngine;
using UnityEngine.Serialization;

namespace Logic.LevelGoals
{
    public class LevelGoalView : MonoBehaviour
    {
        [SerializeField] private CollectibleCoinSpawner _coinSpawner;
        [SerializeField] private AnimalInteraction _animalInteraction;
        [SerializeField] private InterfaceReference<ITransformGrid, MonoBehaviour> _transformGrid;
        [SerializeField] private LevelGoalPanelView _goalPanelView;
        [SerializeField] private bool _isDeactivateOnRelease;
        [FormerlySerializedAs("_temporalGoalPreset")] [SerializeField] private GoalConfig _temporalGoalConfig;

        private LevelGoal _goal;
        private ReleaseInteractionsGrid _releaseInteractions;

        public event Action<AnimalType> ReleaseInteraction
        {
            add => _releaseInteractions.ReleaseInteracted += value;
            remove => _releaseInteractions.ReleaseInteracted -= value;
        }

        private void OnDestroy()
        {
            _releaseInteractions.Dispose();
            _goal.Dispose();
            _goalPanelView.Dispose();
        }

        public void Construct(IGameFactory gameFactory, IAnimalsService animalsService)
        {
            _releaseInteractions = new ReleaseInteractionsGrid(gameFactory, _transformGrid.Value, _temporalGoalConfig.AnimalsToRelease.Keys);
            _goal = new LevelGoal(animalsService, _animalInteraction, _releaseInteractions, _temporalGoalConfig, _coinSpawner, _isDeactivateOnRelease);
            _goalPanelView.Construct(gameFactory, _temporalGoalConfig, _goal.Progress);
        }
    }
}