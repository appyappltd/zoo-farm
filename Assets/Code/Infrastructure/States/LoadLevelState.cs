using Cameras;
using Infrastructure.Factory;
using Logic;
using Logic.Wallet;
using Player;
using Services.Camera;
using Services.Input;
using Services.PersistentProgress;
using Ui;
using Ui.Elements;
using Ui.Elements.Buttons;
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

        public LoadLevelState(GameStateMachine gameStateMachine, LoadingCurtain curtain, SceneLoader sceneLoader,
            IGameFactory gameFactory, IPlayerInputService inputService, IPersistentProgressService progressService, ICameraOperatorService cameraService)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _inputService = inputService;
            _progressService = progressService;
            _curtain = curtain;
            _cameraService = cameraService;
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
            _cameraService.RegisterCamera(cameraMovement);
            _cameraService.SetAsDefault(heroTransform);
        }

        private GameObject InitHero()
        {
            GameObject hero = _gameFactory.CreateHero(Vector3.zero);
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
