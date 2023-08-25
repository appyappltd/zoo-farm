using System;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Services.Animals;

namespace Logic.LevelGoals
{
    public class LevelGoal : IDisposable
    {
        private readonly GoalProgress _goalProgress;
        private readonly IAnimalsService _animalsService;
        private readonly ReleaseInteractionsGrid _releaseInteractionsGrid;
        private readonly bool _isDeactivateOnRelease;

        public LevelGoal(IAnimalsService animalsService, ReleaseInteractionsGrid releaseInteractionsGrid, GoalPreset goalPreset, bool isDeactivateOnRelease)
        {
            _animalsService = animalsService;
            _releaseInteractionsGrid = releaseInteractionsGrid;
            _isDeactivateOnRelease = isDeactivateOnRelease;
            _goalProgress = new GoalProgress(goalPreset);
            
            _releaseInteractionsGrid.ReleaseInteracted += OnReleaseInteracted;
            
            _animalsService.Registered += OnRegistered;
            
            if (_isDeactivateOnRelease)
                _animalsService.Released += OnReleased;
        }

        public IGoalProgressView Progress => _goalProgress;

        public void Dispose()
        {
            _releaseInteractionsGrid.Dispose();
            
            _animalsService.Registered -= OnRegistered;
            
            if (_isDeactivateOnRelease)
                _animalsService.Released -= OnReleased;
        }

        private void OnReleaseInteracted(AnimalType type) =>
            _goalProgress.AddToReleased(type);
        
        private void OnReleased(IAnimal animal)
        {
            AnimalType registeredType = animal.AnimalId.Type;

            if (IsMissing(registeredType))
                _releaseInteractionsGrid.DeactivateZone(registeredType);
        }

        private void OnRegistered(IAnimal animal)
        {
            AnimalType registeredType = animal.AnimalId.Type;

            if (IsSingle(registeredType))
                _releaseInteractionsGrid.ActivateZone(registeredType);
        }

        private bool IsSingle(AnimalType animalType) =>
            _animalsService.GetAnimalsCount(animalType).Total == 1;

        private bool IsMissing(AnimalType animalType) =>
            _animalsService.GetAnimalsCount(animalType).Total < 1;
    }
}