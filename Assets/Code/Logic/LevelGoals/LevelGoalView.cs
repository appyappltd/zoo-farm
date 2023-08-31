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

namespace Logic.LevelGoals
{
    public class LevelGoalView : MonoBehaviour
    {
        [SerializeField] private CollectibleCoinSpawner _coinSpawner;
        [SerializeField] private AnimalInteraction _animalInteraction;
        [SerializeField] private InterfaceReference<ITransformGrid, MonoBehaviour> _transformGrid;
        [SerializeField] private LevelGoalPanelView _goalPanelView;
        [SerializeField] private bool _isDeactivateOnRelease;

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

        public void Construct(IGameFactory gameFactory, IAnimalsService animalsService, GoalConfig goalConfig)
        {
            _releaseInteractions = new ReleaseInteractionsGrid(gameFactory, _transformGrid.Value, goalConfig.GetAnimalsToRelease());
            _goal = new LevelGoal(animalsService, _animalInteraction, _releaseInteractions, goalConfig, _coinSpawner, _isDeactivateOnRelease);
            _goalPanelView.Construct(gameFactory, _goal);
        }
    }
}