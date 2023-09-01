using System.Collections.Generic;
using System.Linq;
using Data;
using Infrastructure.Factory;
using Logic.Animals;
using Logic.Interactions;
using Logic.TransformGrid;
using NTC.Global.System;
using Services.PersistentProgress;
using Services.StaticData;
using UnityEngine;

namespace Logic.CellBuilding.Foundations
{
    public class HouseFoundation : Foundation<AnimalType>
    {
        private readonly IAnimalCounter _animalCounter;
        
        public HouseFoundation(IStaticDataService staticData, IPersistentProgressService persistentProgress,
            IGameFactory gameFactory, ITransformGrid transformGrid, IAnimalCounter animalCounter,
            Transform selfTransform) : base(staticData, persistentProgress, gameFactory, selfTransform, transformGrid)
        {
            _animalCounter = animalCounter;
        }

        protected override IReadOnlyCollection<AnimalType> GetAvailableTypes() =>
            _animalCounter.GetAvailableAnimalTypes();

        protected override IReadOnlyCollection<AnimalType> GetAllPossibleTypes() =>
            StaticData.GoalConfigForLevel(PersistentProgress.Progress.LevelData.LevelKey)
                .GetAnimalsToRelease().ToArray();

        protected override void CreateBuilding(ChoseInteractionProvider choseZone, AnimalType associatedType) => 
            GameFactory.CreateAnimalHouse(SelfTransform.position, SelfTransform.rotation, associatedType);

        protected override ChoseInteractionProvider CreateChoseZone(AnimalType withType)
        {
            ChoseInteractionProvider choseZone =
                GameFactory.CreateChoseInteraction(Vector3.zero, Quaternion.identity, withType);
            choseZone.gameObject.Disable();
            return choseZone;
        }
    }
}