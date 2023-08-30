using System;
using System.Collections.Generic;
using Observables;
using Logic.Interactions;
using Data;
using Infrastructure;
using Infrastructure.Factory;
using Logic.Animals;
using Logic.Player;
using Logic.TransformGrid;
using NTC.Global.System;
using Services.StaticData;
using UnityEngine;

namespace Logic.Houses
{
    public class HouseFoundation : IDisposable
    {
        private readonly ITransformGrid _transformGrid;
        private readonly IAnimalCounter _animalCounter;
        private readonly IStaticDataService _staticData;
        private readonly IGameFactory _gameFactory;

        private CompositeDisposable _disposable = new CompositeDisposable();
        
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

        public void Dispose() =>
            _disposable?.Dispose();

        public void ShowBuildChoice()
        {
            foreach (var animalType in _animalCounter.GetAvailableAnimalTypes())
            {
                _transformGrid.AddCell(_choseHouseInteractions[animalType].transform);
            }
        }

        public void HideBuildChoice() =>
            _transformGrid.RemoveAll();

        private void CreateAllHouseInteractions()
        {
            foreach (AnimalType availableAnimalType in _staticData.GoalConfigForLevel(LevelNames.First).AnimalsToRelease.Keys)
            {
                ChoseHouseInteractionProvider choseHouseZone = _gameFactory.CreateChoseInteraction(Vector3.zero, Quaternion.identity, availableAnimalType);
                choseHouseZone.gameObject.Disable();
                choseHouseZone.Construct(availableAnimalType);
                _choseHouseInteractions.Add(availableAnimalType, choseHouseZone);

                Subscribe(choseHouseZone);
            }
        }

        private void Subscribe(ChoseHouseInteractionProvider choseHouseZone)
        {
            void OnHouseChosen(Hero hero)
            {
                Transform transform = choseHouseZone.transform;
                _gameFactory.CreateAnimalHouse(transform.position, transform.rotation,
                    choseHouseZone.AssociatedAnimalType);
            }

            choseHouseZone.Interaction.Interacted += OnHouseChosen;
            _disposable.Add(new EventDisposer(() => choseHouseZone.Interaction.Interacted -= OnHouseChosen));
        }
    }
}