using System;
using Logic.Animals;
using Services.Animals;

namespace Logic.LevelGoals
{
    public class LevelGoal : IDisposable
    {
        private readonly GoalProgress _goalProgress;
        private readonly IAnimalsService _animalsService;
        private readonly ReleaseInteractionsGrid _releaseInteractionsGrid;

        public LevelGoal(IAnimalsService animalsService, GoalPreset goalPreset, ReleaseInteractionsGrid releaseInteractionsGrid)
        {
            _animalsService = animalsService;
            _releaseInteractionsGrid = releaseInteractionsGrid;
            _goalProgress = new GoalProgress(goalPreset);
            
            _releaseInteractionsGrid.ReleaseInteracted += OnReleaseInteracted;
        }

        public void Dispose() =>
            _releaseInteractionsGrid.Dispose();

        private void OnReleaseInteracted(AnimalType type) =>
            _goalProgress.AddToReleased(type);
    }
}