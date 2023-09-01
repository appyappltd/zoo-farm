using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Factory;
using Logic.Animals.AnimalFeeders;
using Logic.Foods.FoodSettings;
using Logic.Interactions;
using Logic.TransformGrid;
using NTC.Global.System;
using Services.Animals;
using Services.Feeders;
using Services.PersistentProgress;
using Services.StaticData;
using UnityEngine;

namespace Logic.CellBuilding.Foundations
{
    public class FeederFoundation : Foundation<FoodId>
    {
        private readonly IAnimalsService _animalsService;
        private readonly IAnimalFeederService _feederService;

        public FeederFoundation(IStaticDataService staticData, IPersistentProgressService persistentProgress,
            IGameFactory gameFactory, ITransformGrid transformGrid, Transform selfTransform,
            IAnimalsService animalsService, IAnimalFeederService feederService) : base(staticData, persistentProgress,
            gameFactory, selfTransform,
            transformGrid)
        {
            _animalsService = animalsService;
            _feederService = feederService;
        }

        protected override IReadOnlyCollection<FoodId> GetAvailableTypes() =>
            _animalsService.Animals.Select(animal => animal.AnimalId.EdibleFood).Distinct().ToArray();

        //TODO: Заменить на нормальную реализацию
        protected override IReadOnlyCollection<FoodId> GetAllPossibleTypes()
        {
            var readOnlyCollection = Enum.GetValues(typeof(FoodId)).Cast<FoodId>().ToList();
            readOnlyCollection = readOnlyCollection.GetRange(1, readOnlyCollection.Count - 2);
            return readOnlyCollection;
        }

        protected override void CreateBuilding(ChoseInteractionProvider choseZone, FoodId associatedType)
        {
            _feederService.Register(GameFactory
                .CreateFeeder(SelfTransform.position, SelfTransform.rotation, associatedType)
                .GetComponent<AnimalFeederView>().Feeder);
        }

        protected override ChoseInteractionProvider CreateChoseZone(FoodId withType)
        {
            ChoseInteractionProvider choseZone =
                GameFactory.CreateChoseInteraction(Vector3.zero, Quaternion.identity, withType);
            choseZone.gameObject.Disable();
            return choseZone;
        }
    }
}