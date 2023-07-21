using Cameras;
using Infrastructure.Factory;
using Logic;
using Logic.Payment;
using Logic.Player;
using Logic.SpawnPlaces;
using Player;
using Services.Camera;
using Services.Effects;
using Services.Input;
using Services.MedicalBeds;
using Services.PersistentProgress;
using Services.Pools;
using Services.StaticData;
using Ui;
using Ui.Elements.Buttons;
using Ui.Factory;
using UnityEngine;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;
        private readonly IPlayerInputService _inputService;
        private readonly IPersistentProgressService _progressService;
        private readonly LoadingCurtain _curtain;
        private readonly ICameraOperatorService _cameraService;
        private readonly IUIFactory _uiFactory;
        private readonly IStaticDataService _staticData;
        private readonly IPoolService _poolService;
        private readonly IMedicalBedsReporter _medicalBedsReporter;

        public LoadLevelState(GameStateMachine gameStateMachine, LoadingCurtain curtain, SceneLoader sceneLoader,
            IGameFactory gameFactory, IPlayerInputService inputService, IPersistentProgressService progressService,
            ICameraOperatorService cameraService, IUIFactory uiFactory, IStaticDataService staticData,
            IPoolService poolService, IMedicalBedsReporter medicalBedsReporter)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _inputService = inputService;
            _progressService = progressService;
            _curtain = curtain;
            _cameraService = cameraService;
            _uiFactory = uiFactory;
            _staticData = staticData;
            _poolService = poolService;
            _medicalBedsReporter = medicalBedsReporter;
        }

        public void Enter(string payload)
        {
            _curtain.Show();
            _poolService.DestroyAllPools();
            _gameFactory.Cleanup();
            _medicalBedsReporter.Cleanup();
            _sceneLoader.Load(payload, OnLoaded);
        }

        public void Exit() =>
            _curtain.Hide();

        private void OnLoaded()
        {
            _gameFactory.WarmUp();
            InitialGameWorld();
            GameObject hero = InitHero();
            FollowCamera(hero.transform);
            InitialHud(hero);
            InitUIRoot();
            InformProgressReaders();
            _stateMachine.Enter<GameLoopState>();
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
                progressReader.LoadProgress(_progressService.Progress);
        }

        private void FollowCamera(Transform heroTransform)
        {
            CameraMovement cameraMovement = Camera.main.GetComponent<CameraMovement>();
            cameraMovement.Follow(heroTransform);
            cameraMovement.transform.parent.position = heroTransform.position;
            _cameraService.RegisterCamera(cameraMovement);
            _cameraService.SetAsDefault(heroTransform);
        }

        private void InitUIRoot() =>
            _uiFactory.CreateUIRoot();

        private GameObject InitHero()
        {
            Transform heroSpawnPlace = _staticData.SpawnPlaceById(SpawnPlaceId.Player);
            GameObject hero = _gameFactory.CreateHero(heroSpawnPlace.position);
            hero.GetComponent<Hero>().Construct(_inputService);
            return hero;
        }

        private void InitialHud(GameObject hero)
        {
            var hud = _gameFactory.CreateHud();
            hud.GetComponent<Canvas>().worldCamera = Camera.main;

            var inputReader = hud.GetComponentInChildren<IInputReader>();
            _inputService.RegisterInputReader(inputReader);

            IWallet wallet = hero.GetComponent<Hero>().Wallet;
            hud.GetComponentInChildren<MoneyView>().Construct(wallet.Account);
            hud.GetComponentInChildren<AddCoinsButton>().Construct(wallet);
        }

        private void InitialGameWorld()
        {
        }
    }
}