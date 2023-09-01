using Infrastructure.Factory;
using Logic.Houses;
using Services;
using Services.Animals;
using Services.Feeders;
using Services.PersistentProgress;
using Services.StaticData;

namespace Logic.CellBuilding.Foundations.Views
{
    public class FeederFoundationView : FoundationView
    {
        private IStaticDataService _staticData;
        private IPersistentProgressService _persistentProgress;
        private IGameFactory _gameFactory;
        private IAnimalsService _animalsService;
        private IAnimalFeederService _feederService;

        protected override void OnAwake()
        {
            Construct(
                AllServices.Container.Single<IStaticDataService>(),
                AllServices.Container.Single<IPersistentProgressService>(),
                AllServices.Container.Single<IGameFactory>(),
                AllServices.Container.Single<IAnimalsService>(),
                AllServices.Container.Single<IAnimalFeederService>());
        }

        private void Construct(IStaticDataService staticData, IPersistentProgressService persistentProgress, IGameFactory gameFactory, IAnimalsService animalsService, IAnimalFeederService feederService)
        {
            _feederService = feederService;
            _animalsService = animalsService;
            _gameFactory = gameFactory;
            _persistentProgress = persistentProgress;
            _staticData = staticData;
        }

        protected override IFoundation CreateFoundation() =>
            new FeederFoundation(_staticData, _persistentProgress, _gameFactory, TransformGrid.Value, transform, _animalsService, _feederService);
    }
}