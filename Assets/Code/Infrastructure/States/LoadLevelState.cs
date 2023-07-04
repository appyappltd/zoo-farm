using Cameras;
using Infrastructure.Factory;
using Logic;
using Logic.Payment;
using Logic.SpawnPlaces;
using Player;
using Services.Camera;
using Services.Input;
using Services.PersistentProgress;
using Services.StaticData;
using Ui;
using Ui.Elements;
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

        public LoadLevelState(GameStateMachine gameStateMachine, LoadingCurtain curtain, SceneLoader sceneLoader,
            IGameFactory gameFactory, IPlayerInputService inputService, IPersistentProgressService progressService,
            ICameraOperatorService cameraService, IUIFactory uiFactory, IStaticDataService staticData)
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
        }

        public void Enter(string payload)
        {
            _curtain.Show();
            _gameFactory.Cleanup();
            _gameFactory.WarmUp();
            _sceneLoader.Load(payload, OnLoaded);
        }

        public void Exit() =>
            _curtain.Hide();

        private void OnLoaded()
        {
            InitialGameWorld();
            GameObject hero = InitHero();
            FollowCamera(hero.transform);
            InitialHud(hero);
            InitUIRoot();
            _stateMachine.Enter<GameLoopState>();
            InformProgressReaders();
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
            hero.GetComponent<PlayerMovement>().Construct(_inputService);
            return hero;
        }

        private void InitialHud(GameObject hero)
        {
            var hud = _gameFactory.CreateHud();
            hud.GetComponent<Canvas>().worldCamera = Camera.main;

            var inputReader = hud.GetComponentInChildren<IInputReader>();
            _inputService.RegisterInputReader(inputReader);

            Wallet wallet = hero.GetComponent<HeroWallet>().Wallet;
            hud.GetComponentInChildren<MoneyView>().Construct(wallet.Account);
            hud.GetComponentInChildren<AddCoinsButton>().Construct(wallet);
        }

        private void InitialGameWorld()
        {
        }
    }
}