using Data;
using Infrastructure.Factory;
using Logic.Houses;
using Services;
using Services.Animals;
using Services.PersistentProgress;
using Services.StaticData;

namespace Logic.CellBuilding.Foundations.Views
{
    public class HouseFoundationView : FoundationView
    {
        private IStaticDataService _staticData;
        private IPersistentProgressService _persistentProgress;
        private IGameFactory _gameFactory;
        private IAnimalCounter _animalCounter;

        protected override void OnAwake()
        {
            Construct(
                AllServices.Container.Single<IStaticDataService>(),
                AllServices.Container.Single<IPersistentProgressService>(),
                AllServices.Container.Single<IGameFactory>(),
                AllServices.Container.Single<IAnimalsService>().AnimalCounter);

        }

        private void Construct(IStaticDataService staticData, IPersistentProgressService persistentProgress, IGameFactory gameFactory, IAnimalCounter animalCounter)
        {
            _animalCounter = animalCounter;
            _gameFactory = gameFactory;
            _persistentProgress = persistentProgress;
            _staticData = staticData;
        }
        protected override IFoundation CreateFoundation() =>
            new HouseFoundation(_staticData, _persistentProgress, _gameFactory, TransformGrid.Value, _animalCounter, transform);
    }
}