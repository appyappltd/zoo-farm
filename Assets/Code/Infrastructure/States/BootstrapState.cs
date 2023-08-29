using Infrastructure.AssetManagement;
using Infrastructure.Factory;
using Logic.Animals;
using Services;
using Services.AnimalHouses;
using Services.Animals;
using Services.Breeding;
using Services.Camera;
using Services.Effects;
using Services.Food;
using Services.Input;
using Services.MedicalBeds;
using Services.PersistentProgress;
using Services.Pools;
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
            Register<ICoroutineRunner>(_coroutineRunner);
            Register<IPlayerInputService>(new PlayerInputService());
            Register<IRandomService>(new RandomService());
            Register<IPoolService>(new PoolService());
            Register<IAssetProvider>(new AssetProvider());
            Register<IPersistentProgressService>(new PersistentProgressService());
            Register<IAnimalHouseService>(new AnimalHouseService());
            Register<IFoodService>(new FoodService());
            Register<IMedicalBedsReporter>(new MedicalBedsReporter());
            Register<ICameraOperatorService>(new CameraOperatorService());
            Register<IEffectService>(new EffectService(Get<IPoolService>()));
            Register<IAnimalsService>(
                new AnimalsService(
                    Get<IAnimalHouseService>()));
            Register<IGameFactory>(
                new GameFactory(
                    Get<IAssetProvider>(),
                    Get<IRandomService>(),
                    Get<IPersistentProgressService>(),
                    Get<IStaticDataService>(),
                    Get<IAnimalsService>(),
                    Get<IPoolService>(),
                    Get<IMedicalBedsReporter>(),
                    Get<IAnimalHouseService>()));
            Register<IAnimalBreedService>(
                new AnimalBreedService(Get<IEffectService>(),
                    Get<IGameFactory>(),
                    Get<IAnimalHouseService>(),
                    Get<IAnimalsService>()));
            Register<IUIFactory>(
                new UIFactory(
                    Get<IAssetProvider>(),
                    Get<IStaticDataService>(),
                    Get<IPersistentProgressService>(),
                    Get<IAnimalsService>(),
                    Get<IAnimalHouseService>()
                ));
            Register<IWindowService>(
                new WindowService(
                    Get<IUIFactory>()));
            Register<ISaveLoadService>(
                new SaveLoadService(
                    Get<IPersistentProgressService>(),
                    Get<IGameFactory>()));
        }

        private void Register<TService>(TService newService) where TService : IService =>
            _services.RegisterSingle(newService);

        private TService Get<TService>() where TService : IService =>
            _services.Single<TService>();

        private void RegisterStaticDataService()
        {
            IStaticDataService staticData = new StaticDataService();
            staticData.Load();
            Register(staticData);
        }

        private void EnterLoadLevel() =>
            _stateMachine.Enter<LoadProgressState>();
    }
}