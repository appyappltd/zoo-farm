using System;
using Infrastructure.Factory;
using Logic.TransformGrid;
using Services.Animals;
using UnityEngine;

namespace Logic.LevelGoals
{
    public class LevelGoalView : MonoBehaviour
    {
        [SerializeField] private ITransformGrid _transformGrid;
        [SerializeField] private GoalPreset _temporalGoalPreset;
        
        private IGameFactory _gameFactory;

        private LevelGoal _goal;
        private ReleaseInteractionsGrid _releaseInteractions;
        private IAnimalsService _animalsService;

        private void OnDestroy()
        {
            _releaseInteractions.Dispose();
        }

        public void Construct(IGameFactory gameFactory, IAnimalsService animalsService)
        {
            _animalsService = animalsService;
            _gameFactory = gameFactory;
            _releaseInteractions = new ReleaseInteractionsGrid(_gameFactory, _transformGrid, _temporalGoalPreset.AmountAnimalToRelease.Keys);
            _goal = new LevelGoal(animalsService, _temporalGoalPreset, _releaseInteractions);
        }
        
        
    }
}