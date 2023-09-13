using System;
using Data;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Logic.Interactions;
using Logic.Spawners;
using Services.Animals;
using UnityEngine;

namespace Logic.LevelGoals
{
    public class LevelGoal : IDisposable
    {
        private readonly IAnimalsService _animalsService;
        private readonly ReleaseInteractionsGrid _releaseInteractionsGrid;
        private readonly GoalConfig _goalConfig;
        private readonly CollectibleCoinSpawner _coinSpawner;
        private readonly AnimalInteraction _animalInteraction;
        private readonly bool _isDeactivateOnRelease;

        private GoalProgress _goalProgress;
        private int _currentGoalStage;

        public event Action Updated = () => { };

        public IGoalProgressView Progress => _goalProgress;

        public LevelGoal(IAnimalsService animalsService, AnimalInteraction animalInteraction,
            ReleaseInteractionsGrid releaseInteractionsGrid, GoalConfig goalConfig, CollectibleCoinSpawner coinSpawner, bool isDeactivateOnRelease)
        {
            _animalInteraction = animalInteraction;
            _animalsService = animalsService;
            _releaseInteractionsGrid = releaseInteractionsGrid;
            _goalConfig = goalConfig;
            _coinSpawner = coinSpawner;
            _isDeactivateOnRelease = isDeactivateOnRelease;
            
            
            UpdateProgress();
            _animalInteraction.Interacted += OnAnimalPassGates;
            _animalsService.AnimalCounter.Updated += OnAnimalCountUpdated;
        }

        private void OnAnimalCountUpdated(AnimalType type, AnimalCountData counts)
        {
            if (counts.ReleaseReady <= 0)
                _releaseInteractionsGrid.DeactivateZone(type);
            else
                _releaseInteractionsGrid.ActivateZone(type);
        }

        private void UpdateProgress()
        {
            _goalProgress = new GoalProgress(_goalConfig.Goals[_currentGoalStage]);
            _goalProgress.Compleated += OnGoalCompleated;
        }

        public void Dispose()
        {
            _releaseInteractionsGrid.Dispose();
            
            _goalProgress.Compleated -= OnGoalCompleated;
            _animalInteraction.Interacted -= OnAnimalPassGates;
            _animalsService.AnimalCounter.Updated -= OnAnimalCountUpdated;
        }

        public SingleGoalData GetCurrentGoal() =>
            _goalConfig.Goals[_currentGoalStage];

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

        private void OnGoalCompleated()
        {
            _coinSpawner.Spawn(_goalConfig.Goals[_currentGoalStage].CashRewardForCompletingGoal);
            _currentGoalStage++;

            if (_currentGoalStage > _goalConfig.Goals.Count - 1)
            {
#if DEBUG
                Debug.LogWarning("All Goals compete");
#endif
                return;
            }

            UpdateGoal();
        }

        private void UpdateGoal()
        {
            UpdateProgress();
            Updated.Invoke();
        }

        private bool IsSingle(AnimalType animalType) =>
            _animalsService.AnimalCounter.GetAnimalCountData(animalType).Total == 1;

        private bool IsMissing(AnimalType animalType) =>
            _animalsService.AnimalCounter.GetAnimalCountData(animalType).Total < 1;
    }
}