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
using Object = UnityEngine.Object;

namespace Logic.Houses
{
    public class HouseFoundation : IDisposable
    {
        private readonly IStaticDataService _staticData;
        private readonly IGameFactory _gameFactory;
        private readonly ITransformGrid _transformGrid;
        private readonly IAnimalCounter _animalCounter;
        private readonly Transform _selfTransform;

        private CompositeDisposable _disposable = new CompositeDisposable();
        
        private Dictionary<AnimalType, ChoseHouseInteractionProvider> _choseHouseInteractions = new Dictionary<AnimalType, ChoseHouseInteractionProvider>();

        public HouseFoundation(IStaticDataService staticData, IGameFactory gameFactory, ITransformGrid transformGrid,
            IAnimalCounter animalCounter, Transform selfTransform)
        {
            _staticData = staticData;
            _gameFactory = gameFactory;
            _transformGrid = transformGrid;
            _animalCounter = animalCounter;
            _selfTransform = selfTransform;

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
            foreach (AnimalType availableAnimalType in _staticData.GoalConfigForLevel(LevelNames.First).GetAnimalsToRelease())
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
                _gameFactory.CreateAnimalHouse(_selfTransform.position, _selfTransform.rotation,
                    choseHouseZone.AssociatedAnimalType);
                
                Object.Destroy(_selfTransform.gameObject);
            }

            choseHouseZone.Interaction.Interacted += OnHouseChosen;
            _disposable.Add(new EventDisposer(() => choseHouseZone.Interaction.Interacted -= OnHouseChosen));
        }
    }
}