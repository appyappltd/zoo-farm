using Data;
using Infrastructure.Factory;
using Logic;
using Services.Input;
using Services.PersistentProgress;
using Services.SaveLoad;
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

        public LoadLevelState(GameStateMachine gameStateMachine, LoadingCurtain curtain, SceneLoader sceneLoader,
            IGameFactory gameFactory, IPlayerInputService inputService, IPersistentProgressService progressService)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _inputService = inputService;
            _progressService = progressService;
            _curtain = curtain;
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
            InitialHud();
            _stateMachine.Enter<GameLoopState>();
            InformProgressReaders();
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReaderGeneric progressReader in _gameFactory.ProgressReadersGeneric)
                progressReader.LoadProgress(_progressService);
        }
        
        private void FollowCamera(Transform transform)
        {
            Camera.main.GetComponent<CameraMovement>().Construct(transform);
        }

        private GameObject InitHero()
        {
            GameObject hero = _gameFactory.CreateHero(Vector3.zero);
            hero.GetComponent<PlayerMovement>().Construct(_inputService);
            return hero;
        }

        private void InitialHud()
        {
            var hud = _gameFactory.CreateHud();
            hud.GetComponent<Canvas>().worldCamera = Camera.main;

            var inputReader = hud.GetComponentInChildren<IInputReader>();
            _inputService.RegisterInputReader(inputReader);
        }

        private void InitialGameWorld()
        {
        }
    }
}