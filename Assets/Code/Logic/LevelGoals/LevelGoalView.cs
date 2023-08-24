using System;
using AYellowpaper;
using Infrastructure.Factory;
using Logic.Animals;
using Logic.TransformGrid;
using Services.Animals;
using Ui.LevelGoalPanel;
using UnityEngine;

namespace Logic.LevelGoals
{
    public class LevelGoalView : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<ITransformGrid, MonoBehaviour> _transformGrid;
        [SerializeField] private LevelGoalPanelView _goalPanelView;
        [SerializeField] private GoalPreset _temporalGoalPreset;

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
        }

        public void Construct(IGameFactory gameFactory, IAnimalsService animalsService)
        {
            _releaseInteractions = new ReleaseInteractionsGrid(gameFactory, _transformGrid.Value, _temporalGoalPreset.AnimalsToRelease.Keys);
            _goal = new LevelGoal(animalsService, _releaseInteractions, _temporalGoalPreset);
            _goalPanelView.Construct(gameFactory, _temporalGoalPreset, _goal.Progress);
        }
    }
}