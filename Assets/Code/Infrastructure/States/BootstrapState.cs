using Infrastructure.AssetManagement;
using Infrastructure.Factory;
using Services;
using Services.AnimalHouse;
using Services.Input;
using Services.PersistentProgress;
using Services.Randomizer;
using Services.SaveLoad;
using Services.StaticData;

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
            _services.RegisterSingle<IGameFactory>(
                new GameFactory(
                    _services.Single<IAssetProvider>(),
                    _services.Single<IRandomService>(),
                    _services.Single<IPersistentProgressService>()));
            _services.RegisterSingle<IAnimalHouseService>(new AnimalHouseService(_services.Single<IGameFactory>()));
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