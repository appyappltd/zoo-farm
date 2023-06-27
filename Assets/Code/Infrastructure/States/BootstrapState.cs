using Infrastructure.AssetManagement;
using Infrastructure.Factory;
using Services;
using Services.AnimalHouses;
using Services.Animals;
using Services.Camera;
using Services.Input;
using Services.PersistentProgress;
using Services.Randomizer;
using Services.SaveLoad;
using Services.StaticData;
using Ui.Factory;
using Ui.Services;

namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        private readonly ICoroutineRunner _coroutineRunner;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services,
            ICoroutineRunner coroutineRunner)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            _coroutineRunner = coroutineRunner;

            RegisterServices();
        }

        public void Enter() =>
            _sceneLoader.Load(LevelNames.Initial, EnterLoadLevel);

        public void Exit()
        {
        }

        private void RegisterServices()
        {
            RegisterStaticDataService();
            _services.RegisterSingle<ICoroutineRunner>(_coroutineRunner);
            _services.RegisterSingle<IPlayerInputService>(new PlayerInputService());
            _services.RegisterSingle<IRandomService>(new RandomService());
            _services.RegisterSingle<IAssetProvider>(new AssetProvider());
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            _services.RegisterSingle<IAnimalHouseService>(new AnimalHouseService());
            _services.RegisterSingle<IAnimalsService>(
                new AnimalsService(_services.Single<IAnimalHouseService>()));
            _services.RegisterSingle<ICameraOperatorService>(new CameraOperatorService());
            _services.RegisterSingle<IGameFactory>(
                new GameFactory(
                    _services.Single<IAssetProvider>(),
                    _services.Single<IRandomService>(),
                    _services.Single<IPersistentProgressService>(),
                    _services.Single<IStaticDataService>(),
                    _services.Single<IAnimalsService>()));
            _services.RegisterSingle<IUIFactory>(
                new UIFactory(
                    _services.Single<IAssetProvider>(),
                    _services.Single<IStaticDataService>(),
                    _services.Single<IPersistentProgressService>(),
                    _services.Single<IAnimalsService>()
                ));
            _services.RegisterSingle<IWindowService>(
                new WindowService(
                    _services.Single<IUIFactory>()));
            _services.RegisterSingle<ISaveLoadService>(
                new SaveLoadService(
                    _services.Single<IPersistentProgressService>(),
                    _services.Single<IGameFactory>()));
        }

        private void RegisterStaticDataService()
        {
            IStaticDataService staticData = new StaticDataService();
            staticData.Load();
            _services.RegisterSingle(staticData);
        }

        private void EnterLoadLevel() =>
            _stateMachine.Enter<LoadProgressState>();
    }
}