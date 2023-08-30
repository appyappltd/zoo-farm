using System.Collections.Generic;
using Logic.Interactions;
using Data;
using Infrastructure;
using Infrastructure.Factory;
using Logic.Animals;
using Logic.TransformGrid;
using Services.StaticData;
using UnityEngine;

namespace Logic.Houses
{
    public class HouseFoundation
    {
        private readonly ITransformGrid _transformGrid;
        private readonly IAnimalCounter _animalCounter;
        private readonly IStaticDataService _staticData;
        private readonly IGameFactory _gameFactory;

        private Dictionary<AnimalType, ChoseHouseInteractionProvider> _choseHouseInteractions = new Dictionary<AnimalType, ChoseHouseInteractionProvider>();

        public HouseFoundation(IStaticDataService staticData, IGameFactory gameFactory, ITransformGrid transformGrid,
            IAnimalCounter animalCounter)
        {
            _staticData = staticData;
            _gameFactory = gameFactory;
            _transformGrid = transformGrid;
            _animalCounter = animalCounter;

            CreateAllHouseInteractions();
        }

        public void ShowBuildChoice()
        {
            foreach (var animalType in _animalCounter.GetAvailableAnimalTypes())
            {
                _transformGrid.AddCell(_choseHouseInteractions[animalType].transform);
            }
        }

        public void HideBuildChoice()
        {
            _transformGrid.RemoveAll();
        }
        
        private void CreateAllHouseInteractions()
        {
            foreach (var availableAnimalType in _staticData.GoalConfigForLevel(LevelNames.First).AnimalsToRelease.Keys)
            {
                var choseHouseZone = _gameFactory.CreateChoseInteraction(Vector3.zero, Quaternion.identity, availableAnimalType);
                _choseHouseInteractions.Add(availableAnimalType, choseHouseZone);
            }
        }
    }
}