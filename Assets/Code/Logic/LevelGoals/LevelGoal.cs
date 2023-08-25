using System;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Logic.Interactions;
using Services.Animals;

namespace Logic.LevelGoals
{
    public class LevelGoal : IDisposable
    {
        private readonly GoalProgress _goalProgress;
        private readonly IAnimalsService _animalsService;
        private readonly ReleaseInteractionsGrid _releaseInteractionsGrid;
        private readonly bool _isDeactivateOnRelease;
        private AnimalInteraction _animalInteraction;

        public LevelGoal(IAnimalsService animalsService, AnimalInteraction animalInteraction,
            ReleaseInteractionsGrid releaseInteractionsGrid, GoalPreset goalPreset, bool isDeactivateOnRelease)
        {
            _animalInteraction = animalInteraction;
            _animalsService = animalsService;
            _releaseInteractionsGrid = releaseInteractionsGrid;
            _isDeactivateOnRelease = isDeactivateOnRelease;
            _goalProgress = new GoalProgress(goalPreset);

            _animalsService.Registered += OnRegistered;
            animalInteraction.Interacted += OnAnimalPassGates;
        }

        public IGoalProgressView Progress => _goalProgress;

        public void Dispose()
        {
            _releaseInteractionsGrid.Dispose();
            
            _animalsService.Registered -= OnRegistered;
            _animalInteraction.Interacted -= OnAnimalPassGates;
        }


        private void OnAnimalPassGates(IAnimal animal)
        {
            AnimalType releasedType = animal.AnimalId.Type;

            _goalProgress.AddToReleased(releasedType);
            
            if (_isDeactivateOnRelease == false)
                return;

            if (IsMissing(releasedType))
                _releaseInteractionsGrid.DeactivateZone(releasedType);
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